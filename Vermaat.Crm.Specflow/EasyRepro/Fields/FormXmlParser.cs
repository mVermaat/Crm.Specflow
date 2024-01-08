using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Linq;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    internal class FormXmlParser
    {
        private static readonly Dictionary<string, HashSet<string>> _compositeFields;

        static FormXmlParser()
        {
            _compositeFields = new Dictionary<string, HashSet<string>>();

            AddCompositeField("account", "address1_composite");
            AddCompositeField("account", "address2_composite");
            AddCompositeField("account", "address3_composite");

            AddCompositeField("contact", "address1_composite");
            AddCompositeField("contact", "address2_composite");
            AddCompositeField("contact", "address3_composite");
            AddCompositeField("contact", "fullname");

            AddCompositeField("lead", "address1_composite");
            AddCompositeField("lead", "address2_composite");
            AddCompositeField("lead", "address3_composite");
            AddCompositeField("lead", "fullname");


        }

        public static void AddCompositeField(string entityName, string compositeFieldName)
        {
            if (!_compositeFields.TryGetValue(entityName, out var fieldList))
            {
                fieldList = new HashSet<string>();
                _compositeFields.Add(entityName, fieldList);
            }
            fieldList.Add(compositeFieldName);
        }

        public static Dictionary<string, FormFieldSet> ParseForm(UCIApp app, SystemForm form, EntityMetadata metadata)
        {
            var definition = form.FormXml;
            var formFields = new Dictionary<string, FormFieldSet>();
            var metadataDic = metadata.Attributes.ToDictionary(a => a.LogicalName);
            var context = new FormCellParsingContext()
            {
                App = app,
                MetadataDic = metadataDic,
                FormType = form.Type
            };

            foreach (var tab in definition.Tabs)
            {
                context.TabName = tab.Name ?? tab.Id;
                context.TabLabel = tab.Labels.GetLabelInLanguage(app.UILanguageCode, GlobalTestingContext.LanguageCode);
                foreach (var column in tab.Columns)
                {
                    foreach (var section in column.Sections)
                    {
                        context.SectionName = section.Name;
                        context.SectionLabel = section.Labels.GetLabelInLanguage(app.UILanguageCode, GlobalTestingContext.LanguageCode);
                        foreach (var row in section.Rows)
                        {
                            ProcessFormRow(row, metadata, formFields, context);
                        }
                    }
                }
            }

            // Null if there is no header
            if (definition.Header?.Rows != null)
            {
                context.IsHeader = true;
                context.TabLabel = "Header";
                context.TabName = "Header";
                context.SectionName = null;
                context.SectionLabel = null;
                foreach (var row in definition.Header.Rows)
                {
                    ProcessFormRow(row, metadata, formFields, context);
                }
            }

            ParseBusinessProcessFlow(app, metadata, formFields, metadataDic);

            return formFields;
        }

        private static void ParseBusinessProcessFlow(UCIApp app, EntityMetadata metadata, Dictionary<string, FormFieldSet> formFields, Dictionary<string, AttributeMetadata> metadataDic)
        {
            var bpfDefinition = SeleniumCommandProcessor.ExecuteCommand(app, app.SeleniumCommandFactory.CreateGetGetBusinessProcessFlowDefinitionCommand());
            if (bpfDefinition != null)
            {
                foreach (var entity in bpfDefinition.Entities)
                {
                    // in case of multi entity BPF
                    if (entity.EntityLogicalName != metadata.LogicalName)
                        continue;

                    foreach (var step in entity.Stage.Steps)
                    {
                        if (step.StepType != BusinessProcessFlowStepType.Field)
                            continue;

                        var formField = new BusinessProcessFlowFormField(app, metadataDic[step.Name], new FormControl()
                        {
                            AttributeName = step.Name,
                            ControlName = $"{Constants.CRM.BUSINESS_PROCESS_FLOW_CONTROL_PREFIX}{step.StepControlId}"
                        }, entity.Stage.StageDisplayName);

                        if (!formFields.TryGetValue(step.Name, out var formFieldSet))
                        {
                            formFieldSet = new FormFieldSet();
                            formFields.Add(step.Name, formFieldSet);
                        }
                        formFieldSet.Add(formField, "Business Process Flow", entity.Stage.StageDisplayName);
                    }
                }
            }
        }

        private static void ProcessFormRow(FormRow row, EntityMetadata metadata, Dictionary<string, FormFieldSet> formFields,
            FormCellParsingContext context)
        {
            if (row.Cells == null)
                return;

            foreach (var cell in row.Cells)
            {
                // mapcontrol, subgrids and alike will be skipped
                // empty column will be a cell without control
                if (cell == null || cell.IsSpacer || string.IsNullOrEmpty(cell.Control?.AttributeName))
                    continue;

                context.Cell = cell;
                GetFormCellParser(metadata, cell).AddToDictionary(formFields, context);

            }
        }

        private static FormCellParser GetFormCellParser(EntityMetadata metadata, FormCell cell)
        {
            if (_compositeFields.TryGetValue(metadata.LogicalName, out var compositeSet) && compositeSet.Contains(cell.Control.AttributeName))
                return new CompositeFormCellParser();

            return new DefaultFormCellParser();
        }
    }
}
