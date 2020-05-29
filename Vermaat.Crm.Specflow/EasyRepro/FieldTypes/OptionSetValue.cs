using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class OptionSetValue
    {
        public OptionSetValue(int value, string label)
        {
            Value = value;
            Label = label;
        }

        public OptionSet ToOptionSet(AttributeMetadata metadata)
        {
            return new OptionSet { Name = metadata.LogicalName, Value = Label };
        }

        public int Value { get; }
        public string Label { get; }
    }
}
