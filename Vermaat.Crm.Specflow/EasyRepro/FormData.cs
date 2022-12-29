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

            _formFields = InitializeFormData(currentForm);
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

        private Dictionary<string, FormFieldSet> InitializeFormData(SystemForm currentForm)
        {
            var definition = currentForm.FormXml;
            dynamic attributeCollection = _app.WebDriver.ExecuteScript("return Xrm.Page.data.entity.attributes.getAll().map(function(a) { return { name: a.getName(), controls: a.controls.getAll().map(function(c) { return c.getName() }) } })");

            var formFields = new Dictionary<string, FormFieldSet>();
            var metadataDic = _entityMetadata.Attributes.ToDictionary(a => a.LogicalName);


            int tabNr = 0;
            foreach(var tab in definition.Tabs)
            {
                tabNr++;
                int columnNr = 0;
                foreach (var column in tab.Columns)
                {
                    columnNr++;
                    int sectionNr = 0;
                    foreach (var section in column.Sections)
                    {
                        sectionNr++;
                        int rowNr = 0;
                        foreach (var row in section.Rows)
                        {
                            rowNr++;
                            ProcessFormRow(formFields, metadataDic, row);
                        }
                    }
                }
            }

            int headerRowNr = 0;
            foreach(var row in definition.Header.Rows)
            {
                headerRowNr++;
                ProcessFormRow(formFields, metadataDic, row);
            }


            //foreach (var attribute in attributeCollection)
            //{
            //    var controls = new string[attribute["controls"].Count];

            //    for (int i = 0; i < attribute["controls"].Count; i++)
            //    {
            //        controls[i] = attribute["controls"][i];
            //    }

            //    FormField field = CreateFormField(metadataDic[attribute["name"]], controls);
            //    if (field != null)
            //        formFields.Add(attribute["name"], field);
                
            //}

            return formFields;
        }

        private void ProcessFormRow(Dictionary<string, FormFieldSet> formFields, Dictionary<string, AttributeMetadata> metadataDic, FormRow row)
        {
            if (row.Cells == null)
                return;

            foreach (var cell in row.Cells)
            {
                // mapcontrol and alike will be skipped)
                if (cell.IsSpacer || string.IsNullOrEmpty(cell.Control.AttributeName))
                    continue;

                var formField = CreateFormField(metadataDic[cell.Control.AttributeName], cell.Control);
                if (!formFields.TryGetValue(cell.Control.AttributeName, out var formFieldSet))
                {
                    formFieldSet = new FormFieldSet();
                    formFields.Add(cell.Control.AttributeName, formFieldSet);
                }
                formFieldSet.Add(formField);
            }
        }

        private FormField CreateFormField(AttributeMetadata metadata, FormControl cell)
        { 
            if(!cell.ControlName.StartsWith("header"))
            {
                return new BodyFormField(_app, metadata, cell);
            }
            
            return new HeaderFormField(_app, metadata, cell);
        }
    }
}
