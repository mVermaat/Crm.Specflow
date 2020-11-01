using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.FieldTypes
{
    internal class DateTimeValue
    {
        public DateTimeValue(DateTimeAttributeMetadata metadata, DateTime? value)
        {
            if (value.HasValue && metadata.DateTimeBehavior == DateTimeBehavior.UserLocal)
            {
                var offset = GlobalContext.ConnectionManager.CurrentCrmService.UserSettings.TimeZoneInfo.GetUtcOffset(value.Value);
                Value = value.Value.Add(offset);
            }
            else
                Value = value;
        }

        public DateTime? Value { get; }

    }
}
