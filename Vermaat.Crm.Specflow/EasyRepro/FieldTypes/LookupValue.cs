using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class LookupValue
    {
        public LookupValue(EntityReference value)
        {
            Value = value;
        }

        public EntityReference Value { get; }

        public LookupItem ToLookupItem(AttributeMetadata metadata)
        {
            return new LookupItem { Name = metadata.LogicalName, Value = Value?.Name };
        }
    }
}
