using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    internal abstract class FormCellParser
    {
        public abstract void AddToDictionary(Dictionary<string, FormFieldSet> formFields, FormCellParsingContext context);

        protected FormField CreateFormField(FormCellParsingContext context, AttributeMetadata metadata, FormControl cell)
        {
            if (context.IsHeader)
                return new HeaderFormField(context.App, metadata, cell);

            if (context.FormType == SystemFormType.QuickCreate)
                return new QuickCreateBodyFormField(context.App, metadata, cell);

            return new BodyFormField(context.App, metadata, cell, context.TabName, context.SectionName);
        }
    }
}
