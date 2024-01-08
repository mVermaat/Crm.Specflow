using System.Collections.Generic;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    internal class DefaultFormCellParser : FormCellParser
    {
        public override void AddToDictionary(Dictionary<string, FormFieldSet> formFields, FormCellParsingContext context)
        {
            var formField = CreateFormField(context, context.MetadataDic[context.Cell.Control.AttributeName], context.Cell.Control);
            if (!formFields.TryGetValue(context.Cell.Control.AttributeName, out var formFieldSet))
            {
                formFieldSet = new FormFieldSet();
                formFields.Add(context.Cell.Control.AttributeName, formFieldSet);
            }
            formFieldSet.Add(formField, context.TabName, context.SectionName);
        }
    }
}
