using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if(!_compositeFields.TryGetValue(entityName, out var fieldList))
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
                context.TabName = tab.Labels.GetLabelInLanguage(app.UILanguageCode, GlobalTestingContext.LanguageCode);
                foreach (var column in tab.Columns)
                {
                    foreach (var section in column.Sections)
                    {
                        context.SectionName = section.Labels.GetLabelInLanguage(app.UILanguageCode, GlobalTestingContext.LanguageCode);
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
                foreach (var row in definition.Header.Rows)
                {
                    context.IsHeader = true;
                    context.TabName = null;
                    context.SectionName = null;
                    ProcessFormRow(row, metadata, formFields, context);
                }
            }

            return formFields;
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
