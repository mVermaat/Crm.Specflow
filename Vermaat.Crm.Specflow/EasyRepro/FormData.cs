using Microsoft.Crm.Sdk.Messages;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormData
    {
        private readonly UCIApp _app;
        private readonly EntityMetadata _entityMetadata;

        private readonly Dictionary<string, FormField> _formFields;
        private readonly Dictionary<string, FormTab> _formTabs;

        public IReadOnlyDictionary<string, FormTab> Tabs => _formTabs;
        public IReadOnlyDictionary<string, FormField> Fields => _formFields;
        
        public FormField this[string attributeName] => _formFields[attributeName];
        public CommandBarActions CommandBar { get; }

        public FormData(UCIApp app, EntityMetadata entityMetadata)
        {
            _app = app;
            _entityMetadata = entityMetadata;
            CommandBar = new CommandBarActions(_app);

            _formFields = InitializeFormFields();
            _formTabs = InitializeFormTabs();
        }

        public void ClickSubgridButton(string subgridName, string subgridButton)
        {
            _app.Client.ClickSubgridButton(subgridName, subgridButton);
        }

        public bool ContainsField(string fieldLogicalName)
        {
            var containsField = _formFields.ContainsKey(fieldLogicalName);
            Logger.WriteLine($"Field {fieldLogicalName} is on Form: {containsField}");
            return containsField;
        }

        public void ExpandTab(string tabLabel)
        {
            Logger.WriteLine($"Expanding tab {tabLabel}");
            _app.App.Entity.SelectTab(tabLabel);
        }

        public void ExpandHeader()
        {
            Logger.WriteLine("Expanding headers");
            var header = SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Header, string.Empty);
            _app.WebDriver.ClickWhenAvailable(header);
        }

        public string GetErrorDialogMessage()
        {
            Logger.WriteLine("Getting error dialog message");
            return _app.Client.GetErrorDialogMessage();
        }

        public IReadOnlyCollection<FormNotification> GetFormNotifications()
        {
            return _app.Client.GetFormNotifications();
        }

        public Guid GetRecordId()
        {
            Logger.WriteLine("Getting Record Id");
            var id = _app.App.Entity.GetObjectId();
            Logger.WriteLine($"Record ID of current opened record: {id}");
            return id;
        }

        public void Save(bool saveIfDuplicate)
        {
            Logger.WriteLine($"Saving Record");
            try
            {
                _app.App.Entity.Save();
            }
            catch(InvalidOperationException ex)
            {
                throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_FAILED, ex, ex.Message);
            }
            ConfirmDuplicate(saveIfDuplicate);
            WaitUntilSaveCompleted();
        }

        public bool ContainsTab(string tabLabel)
        {
            return _formTabs.ContainsKey(tabLabel);
        }

        public bool TryGetTab(string key, out FormTab tab)
        {
            if (!_formTabs.ContainsKey(key))
            {
                tab = null;
                return false;
            }

            tab = _formTabs[key];
            return true;
        }

        public void FillForm(CrmTestingContext crmContext, Table formData)
        {
            Logger.WriteLine($"Filling form");
            string currentTab = null;
            foreach (var row in formData.Rows)
            {
                Assert.IsTrue(ContainsField(row[Constants.SpecFlow.TABLE_KEY]), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                var field = _formFields[row[Constants.SpecFlow.TABLE_KEY]];

                var newTab = field.GetTabName();
                if (!field.IsFieldInHeaderOnly() && (string.IsNullOrWhiteSpace(currentTab) || currentTab != newTab))
                {
                    ExpandTab(field.GetTabLabel());
                    currentTab = newTab;
                }

                Assert.IsTrue(field.IsVisible(), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't visible");
                Assert.IsFalse(field.IsLocked(), $"Field {row[Constants.SpecFlow.TABLE_KEY]} is read-only");

                field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
            }
        }

        private void WaitUntilSaveCompleted()
        {
            var timeout = DateTime.Now.AddSeconds(20);
            bool saveCompleted = false;
            while (!saveCompleted && DateTime.Now < timeout)
            {
                var footerElement = _app.WebDriver.FindElement(By.XPath("//span[@data-id='edit-form-footer-message']"));

                if (!string.IsNullOrEmpty(footerElement.Text) && footerElement.Text.ToLower() == "saving")
                {
                    Logger.WriteLine("Save not yet completed. Waiting..");
                    Thread.Sleep(500);
                }
                else if(!string.IsNullOrEmpty(footerElement.Text) && footerElement.Text.ToLower() == "unsaved changes")
                {
                    var formNotifications = GetFormNotifications();
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_FAILED, $"Detected Unsaved changes. Form Notifications: {string.Join(", ", formNotifications)}");
                }
                else
                {
                    Logger.WriteLine("Save sucessfull");
                    saveCompleted = true;
                }
            }

            if (!saveCompleted)
                throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_TIMEOUT, 20);
        }

        private void ConfirmDuplicate(bool saveIfDuplicate)
        {
            var element = _app.WebDriver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton]), new TimeSpan(0, 0, 5));

            if(element != null)
            {
                if (saveIfDuplicate)
                {
                    _app.WebDriver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton]));
                }
                else
                {
                    throw new TestExecutionException(Constants.ErrorCodes.DUPLICATE_RECORD_DETECTED);
                }
            }
        }

        private Dictionary<string, FormField> InitializeFormFields()
        {
            dynamic attributeCollection = _app.WebDriver.ExecuteScript("return Xrm.Page.data.entity.attributes.getAll().map(function(a) { return { name: a.getName(), controls: a.controls.getAll().map(function(c) { return c.getName() }) } })");

            var formFields = new Dictionary<string, FormField>();
            var metadataDic = _entityMetadata.Attributes.ToDictionary(a => a.LogicalName);
            foreach (var attribute in attributeCollection)
            {
                var controls = new string[attribute["controls"].Count];

                for (int i = 0; i < attribute["controls"].Count; i++)
                {
                    controls[i] = attribute["controls"][i];
                }

                formFields.Add(attribute["name"], new FormField(this, _app, metadataDic[attribute["name"]], controls));
            }

            return formFields;
        }

        private Dictionary<string, FormTab> InitializeFormTabs()
        {
            dynamic tabCollection = _app.WebDriver.ExecuteScript("return Xrm.Page.ui.tabs.getAll().map(function(t) { return { label: t.getLabel(), visible: t.getVisible() } })");

            var formTabs = new Dictionary<string, FormTab>();
            foreach (var tab in tabCollection)
            {
                formTabs.Add(tab["label"], new FormTab
                {
                    Label = tab["label"],
                    // TODO: Visibility should not be determined using API, it should use the DOM
                    Visible = tab["visible"]
                });
            }

            return formTabs;
        }
    }
}
