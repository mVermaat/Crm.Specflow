using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class DateTimeValue
    {
        public DateTimeValue(DateTimeAttributeMetadata metadata, DateTime value)
        {
            if (metadata.DateTimeBehavior == DateTimeBehavior.UserLocal)
            {
                var offset = GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeZoneInfo.GetUtcOffset(value);
                Value = value.Add(offset);
            }
            else
                Value = value;
        }

        public DateTime Value { get; }

    }
}
