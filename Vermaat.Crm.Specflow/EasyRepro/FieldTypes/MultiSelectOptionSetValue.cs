using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class MultiSelectOptionSetValue
    {


        public MultiSelectOptionSetValue(string[] labelValues)
        {
            LabelValues = labelValues;
        }

        public MultiValueOptionSet ToMultiValueOptionSet(string logicalName)
        {
            return new MultiValueOptionSet { Name = logicalName, Values = LabelValues };
        }

        public string[] LabelValues { get; }


    }
}
