using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.FieldTypes
{
    internal class DoubleValue
    {
        public DoubleValue(double? value)
        {
            Value = value;
        }

        public double? Value { get; }

        public string TextValue => Value?.ToString(
            GlobalContext.ConnectionManager.CurrentCrmService.UserSettings.NumberFormat);
    }
}
