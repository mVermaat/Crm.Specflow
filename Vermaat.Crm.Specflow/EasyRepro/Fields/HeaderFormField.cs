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

        //public override string GetDefaultControl()
        //{
        //    if (_controls.Length == 1)
        //        return _controls[0];
        //    else
        //        return _controls.FirstOrDefault(c => c.StartsWith("header"));
        //}
        public HeaderFormField(UCIApp app, AttributeMetadata attributeMetadata, string control) 
            : base(app, attributeMetadata, control)
        {
        }

        public override bool IsVisible(FormState formState)
        {
            formState.ExpandHeader();
            return base.IsVisible(formState);
        }
    }
}
