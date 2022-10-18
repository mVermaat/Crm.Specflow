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
using Vermaat.Crm.Specflow.EasyRepro.Commands;
using Vermaat.Crm.Specflow.EasyRepro.Fields;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormData
    {
        private readonly UCIApp _app;
        private readonly EntityMetadata _entityMetadata;
        private readonly Dictionary<string, FormField> _formFields;

        public FormField this[string attributeName] => _formFields[attributeName];
        public CommandBarActions CommandBar { get; }

        public FormData(UCIApp app, EntityMetadata entityMetadata)
        {
            _app = app;
            _entityMetadata = entityMetadata;
            CommandBar = new CommandBarActions(_app);

            _formFields = InitializeFormData();
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

        public string GetErrorDialogMessage()
        {
            Logger.WriteLine("Getting error dialog message");
            return _app.Client.GetErrorDialogMessage();
        }

        public IReadOnlyCollection<FormNotification> GetFormNotifications()
        {
            return SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateGetFormNotificationsCommand());
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
            SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateSaveRecordCommand(saveIfDuplicate));
        }

        public void FillForm(CrmTestingContext crmContext, Table formData)
        {
            Logger.WriteLine($"Filling form");
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

                FormField field = CreateFormField(metadataDic[attribute["name"]], controls);
                if (field != null)
                    formFields.Add(attribute["name"], field);
                
            }

            return formFields;
        }

        private FormField CreateFormField(AttributeMetadata metadata, string[] controls)
        {
            if (controls.Length == 0)
                return null;

            // Take the first control that isn't in the header
            for(int i = 0; i < controls.Length; i++)
            {
                if(!controls[i].StartsWith("header"))
                {
                    return new BodyFormField(_app, metadata, controls[i]);
                }
            }
            // If all are in the header, take the first header control
            return new HeaderFormField(_app, metadata, controls[0]);
        }
    }
}
