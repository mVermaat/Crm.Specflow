using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    class HeaderFormField : FormField
    {


        public HeaderFormField(UCIApp app, AttributeMetadata attributeMetadata, string control) 
            : base(app, attributeMetadata, control)
        {
        }

        public override bool IsVisible(FormState formState)
        {
            formState.ExpandHeader();
            return base.IsVisible(formState);
        }

        protected override void SetTextField(string fieldValue)
        {
            App.Client.SetValueFix(LogicalName, fieldValue, FormContextType.Header);
        }


    }
}
