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
using Vermaat.Crm.Specflow.EasyRepro.Fields;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class QuickFormData
    {
        private readonly UCIApp _app;
        private readonly EntityMetadata _entityMetadata;
        private readonly Dictionary<string, FormFieldSet> _formFields;

        public FormField this[string attributeName] => _formFields[attributeName].Get();
        public CommandBarActions CommandBar { get; }

        internal QuickFormData(UCIApp app, EntityMetadata entityMetadata, SystemForm currentForm)
        {
            _app = app;
            _entityMetadata = entityMetadata;
            CommandBar = new CommandBarActions(_app);

            _formFields = FormXmlParser.ParseForm(app, currentForm, entityMetadata);
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
            return SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateGetErrorDialogMessageCommand(true));
        }

        public IReadOnlyCollection<FormNotification> GetFormNotifications()
        {
            return SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateGetFormNotificationsCommand());
        }

        public FormData OpenCreatedRecord(UCIBrowser browser, string childEntityName)
        {
            SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateOpenQuickCreatedRecordCommand(childEntityName));
            return browser.GetFormData(GlobalTestingContext.Metadata.GetEntityMetadata(childEntityName));
        }

        public void Save(bool saveIfDuplicate)
        {
            SeleniumCommandProcessor.ExecuteCommand(_app,_app.SeleniumCommandFactory.CreateSaveQuickCreateRecord(saveIfDuplicate));
        }

        public void FillForm(CrmTestingContext crmContext, Table formData)
        {
            Logger.WriteLine($"Filling Quick create form");
            var formState = new FormState(_app);
            foreach (var row in formData.Rows)
            {
                Assert.IsTrue(ContainsField(row[Constants.SpecFlow.TABLE_KEY]), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                var field = this[row[Constants.SpecFlow.TABLE_KEY]];
                Assert.IsTrue(field.IsVisible(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't visible");
                Assert.IsFalse(field.IsLocked(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} is read-only");

                field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
            }
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
    }
}
