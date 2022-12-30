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
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormData
    {
        private readonly UCIApp _app;
        private readonly EntityMetadata _entityMetadata;
        private readonly Dictionary<string, FormFieldSet> _formFields;

        public FormField this[string attributeName] => _formFields[attributeName].Get();
        public CommandBarActions CommandBar { get; }

        internal FormData(UCIApp app, EntityMetadata entityMetadata, SystemForm currentForm)
        {
            _app = app;
            _entityMetadata = entityMetadata;
            CommandBar = new CommandBarActions(_app);

            _formFields = FormXmlParser.ParseForm(app, currentForm, entityMetadata);
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
                var field = this[row[Constants.SpecFlow.TABLE_KEY]];
                Assert.IsTrue(field.IsVisible(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't visible");
                Assert.IsFalse(field.IsLocked(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} is read-only");

                field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
            }
        }
    }
}
