using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    internal class CompositeFormCellParser : FormCellParser
    {
        public override void AddToDictionary(Dictionary<string, FormFieldSet> formFields, FormCellParsingContext context)
        {
            var fields = SeleniumCommandProcessor.ExecuteCommand(context.App, context.App.SeleniumCommandFactory.CreateGetCompositeControlFieldsCommand(context.Cell.Control.AttributeName));
            var controlPrefix = context.IsHeader ? "header_" : string.Empty;

            foreach(var field in fields)
            {
                var control = new FormControl()
                {
                    AttributeName = field,
                    ControlName = $"{controlPrefix}{context.Cell.Control.ControlName}_compositionLinkControl_{field}",
                };
                var formField = CreateFormField(context, context.MetadataDic[field], control);
                if (!formFields.TryGetValue(field, out var formFieldSet))
                {
                    formFieldSet = new FormFieldSet();
                    formFields.Add(field, formFieldSet);
                }
                formFieldSet.Add(formField, context.TabLabel, context.SectionName);
            }

           
        }
    }
}
