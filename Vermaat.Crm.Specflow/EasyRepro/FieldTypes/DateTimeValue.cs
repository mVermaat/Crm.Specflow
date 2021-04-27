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
        public DateTimeValue(DateTimeAttributeMetadata metadata, DateTime? value)
        {
            if (value.HasValue && metadata.DateTimeBehavior == DateTimeBehavior.UserLocal)
            {
                var offset = GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeZoneInfo.GetUtcOffset(value.Value);
                Value = value.Value.Add(offset);
            }
            else
                Value = value;

            DateOnly = metadata.Format == DateTimeFormat.DateOnly;
        }

        public DateTime? Value { get; }

        public bool DateOnly { get; }

    }
}
