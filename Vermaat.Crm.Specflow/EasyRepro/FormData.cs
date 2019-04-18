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
        private Dictionary<string, FormField> _formFields;

        public FormField this[string attributeName] => _formFields[attributeName];

        public FormData(UCIApp app, EntityMetadata entityMetadata)
        {
            _app = app;
            _entityMetadata = entityMetadata;

            _formFields = InitializeFormData();
        }

        public void Delete()
        {
            _app.App.CommandBar.ClickCommand(_app.ButtonTexts.Delete);
            _app.App.Dialogs.ConfirmationDialog(true);
        }

        public bool ContainsField(string fieldLogicalName)
        {
            return _formFields.ContainsKey(fieldLogicalName);
        }

        public void ExpandTab(string tabLabel)
        {
            _app.App.Entity.SelectTab(tabLabel);
        }


        public Guid GetRecordId()
        {
            return _app.App.Entity.GetObjectId();
        }

        public void Save(bool saveIfDuplicate)
        {
            _app.App.Entity.Save();
            ConfirmDuplicate(saveIfDuplicate);
            WaitUntilSaveCompleted();
        }

        public void FillForm(CrmTestingContext crmContext, Table formData)
        {
            string currentTab = null;
            foreach (var row in formData.Rows)
            {
                Assert.IsTrue(ContainsField(row[Constants.SpecFlow.TABLE_KEY]), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                var field = _formFields[row[Constants.SpecFlow.TABLE_KEY]];

                var newTab = field.GetTabName();
                if(string.IsNullOrWhiteSpace(currentTab) || currentTab != newTab)
                {
                    ExpandTab(field.GetTabLabel());
                    currentTab = newTab;
                }

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
                    Thread.Sleep(2500);
            }
        }

        private void ConfirmDuplicate(bool saveIfDuplicate)
        {
            _app.WebDriver.WaitUntilAvailable(By.XPath("//div[contains(@id,'dialogFooterContainer_')]"), new TimeSpan(0, 0, 5),
                d =>
                {
                    if (saveIfDuplicate)
                    {
                        d.ClickWhenAvailable(By.Id("id-125fc733-aabe-4bd2-807e-fd7b6da72779-4"));
                    }
                    else
                    {
                        throw new ArgumentException("Duplicate found and not selected for save");
                    }
                });
        }

        private Dictionary<string, FormField> InitializeFormData()
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
    }
}
