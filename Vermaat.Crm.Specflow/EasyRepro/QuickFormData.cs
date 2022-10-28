using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro.Commands;
using Vermaat.Crm.Specflow.EasyRepro.Fields;
using Vermaat.Crm.Specflow.FormLoadConditions;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class QuickFormData
    {
        private readonly UCIApp _app;
        private readonly EntityMetadata _entityMetadata;
        private readonly Dictionary<string, QuickCreateBodyFormField> _formFields;

        public QuickCreateBodyFormField this[string attributeName] => _formFields[attributeName];
        public CommandBarActions CommandBar { get; }

        public QuickFormData(UCIApp app, EntityMetadata entityMetadata)
        {
            _app = app;
            _entityMetadata = entityMetadata;
            CommandBar = new CommandBarActions(_app);

            _formFields = InitializeFormData();
        }

        public bool ContainsField(string fieldLogicalName)
        {
            var containsField = _formFields.ContainsKey(fieldLogicalName);
            Logger.WriteLine($"Field {fieldLogicalName} is on Form: {containsField}");
            return containsField;
        }

        public string GetErrorDialogMessage()
        {
            Logger.WriteLine("Getting error dialog message");
            return _app.Client.GetErrorDialogMessage();
        }

        public IReadOnlyCollection<FormNotification> GetFormNotifications()
        {
            return SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateGetFormNotificationsCommand());
        }

        public FormData OpenCreatedRecord(UCIBrowser browser, string childEntityName)
        {
            _app.Client.Execute(BrowserOptionHelper.GetOptions("Open Quick Create Child"), (driver) =>
                {
                    var window = driver.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_QuickCreate_Notification_Window));
                    window.WaitUntilClickable(
                        SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_QuickCreate_OpenChildButton, _app.LocalizedTexts[Constants.LocalizedTexts.QuickCreateViewRecord, _app.UILanguageCode]),
                        TimeSpan.FromSeconds(5),
                        null,
                        () => throw new TestExecutionException(Constants.ErrorCodes.QUICK_CREATE_CHILD_NOT_AVAILABLE)).Click();

                    HelperMethods.WaitForFormLoad(driver, new FormIsOfEntity(childEntityName));
                    return true;
                });
            return browser.GetFormData(GlobalTestingContext.Metadata.GetEntityMetadata(childEntityName));
        }

        public void Save(bool saveIfDuplicate)
        {
            Logger.WriteLine($"Saving Record");
            try
            {
                _app.App.QuickCreate.Save();
            }
            catch (InvalidOperationException ex)
            {
                throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_FAILED, ex, ex.Message);
            }
            ConfirmDuplicate(saveIfDuplicate);
            WaitUntilSaveCompleted();
        }

        public void FillForm(CrmTestingContext crmContext, Table formData)
        {
            Logger.WriteLine($"Filling Quick create form");
            var formState = new FormState(_app);
            foreach (var row in formData.Rows)
            {
                Assert.IsTrue(ContainsField(row[Constants.SpecFlow.TABLE_KEY]), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                var field = _formFields[row[Constants.SpecFlow.TABLE_KEY]];
                Assert.IsTrue(field.IsVisible(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't visible");
                Assert.IsFalse(field.IsLocked(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} is read-only");

                field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
            }
        }

        private void WaitUntilSaveCompleted()
        {
            _app.Client.Execute(BrowserOptionHelper.GetOptions("Open Quick Create Child"), (driver) =>
            {
                var timeout = DateTime.Now.AddSeconds(20);
                var saveCompleted = false;
                while (!saveCompleted && DateTime.Now < timeout)
                {
                    if (driver.HasElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_QuickCreate_Notification_Window)))
                        saveCompleted = true;
                    else
                    {
                        var formNotifications = GetFormNotifications();
                        if (formNotifications.Any())
                        {
                            throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_FAILED, $"Detected Unsaved changes. Form Notifications: {string.Join(", ", formNotifications)}");
                        }
                        Logger.WriteLine("Save not yet completed. Waiting..");
                        Thread.Sleep(250);
                    }
                }

                if (!saveCompleted)
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_TIMEOUT, 20);

                return true;
            });
        }

        public bool IsItQuickCreate()
        {
            var driver = _app.WebDriver;
            var header = driver.FindElements(By.TagName("h1"));
            var headerText = string.Empty;
            if (header.Any())
            {
                if (header.Count == 1)
                {
                    headerText = driver.FindElements(By.TagName("h1")).Single().Text;
                }
                else if (header.Count == 2)
                {
                    headerText = driver.FindElements(By.TagName("h1")).Single(p => p.GetAttribute("title") == string.Empty).Text;
                }
                else
                    throw new NotImplementedException("unhandled header");
            }

            return headerText.Contains("Quick Create");
        }

        private void ConfirmDuplicate(bool saveIfDuplicate)
        {
            var element = _app.WebDriver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton]), new TimeSpan(0, 0, 5));

            if (element != null)
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

        private Dictionary<string, QuickCreateBodyFormField> InitializeFormData()
        {
            dynamic attributeCollection = _app.WebDriver.ExecuteScript("return Xrm.Page.data.entity.attributes.getAll().map(function(a) { return { name: a.getName(), controls: a.controls.getAll().map(function(c) { return c.getName() }) } })");

            var formFields = new Dictionary<string, QuickCreateBodyFormField>();
            var metadataDic = _entityMetadata.Attributes.ToDictionary(a => a.LogicalName);
            foreach (var attribute in attributeCollection)
            {
                var controls = new string[attribute["controls"].Count];

                for (int i = 0; i < attribute["controls"].Count; i++)
                {
                    controls[i] = attribute["controls"][i];
                }

                QuickCreateBodyFormField field = CreateFormField(metadataDic[attribute["name"]], controls);
                if (field != null)
                    formFields.Add(attribute["name"], field);

            }

            return formFields;
        }

        private QuickCreateBodyFormField CreateFormField(AttributeMetadata metadata, string[] controls)
        {
            return controls.Length == 0 ? null : new QuickCreateBodyFormField(_app, metadata, controls[0]);
        }
    }
}
