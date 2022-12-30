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
        public FormXmlParser()
        {

        }

        public static Dictionary<string, FormFieldSet> ParseForm(UCIApp app, SystemForm form, EntityMetadata metadata)
        {
            var definition = form.FormXml;
            var formFields = new Dictionary<string, FormFieldSet>();
            var metadataDic = metadata.Attributes.ToDictionary(a => a.LogicalName);
            var context = new FormCellParsingContext()
            {
                App = app,
                MetadataDic = metadataDic
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

            foreach (var row in definition.Header.Rows)
            {
                context.IsHeader = true;
                context.TabName = null;
                context.SectionName = null;
                ProcessFormRow(row, metadata, formFields, context);
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
                // mapcontrol and alike will be skipped)
                if (cell.IsSpacer || string.IsNullOrEmpty(cell.Control.AttributeName))
                    continue;

                context.Cell = cell;
                GetFormCellParser(metadata, cell).AddToDictionary(formFields, context);
               
            }
        }

        private static FormCellParser GetFormCellParser(EntityMetadata metadata, FormCell cell)
        {
            if (metadata.LogicalName.Equals("contact") && cell.Control.AttributeName.Equals("fullname"))
                return new CompositeFormCellParser();

            return new DefaultFormCellParser();
        }
    }
}
