using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.FieldTypes
{
    internal class IntegerValue
    {


        public IntegerValue(int? value)
        {
            Value = value;
        }

        public int? Value { get; }

        public string TextValue => Value?.ToString(
            GlobalContext.ConnectionManager.CurrentCrmService.UserSettings.NumberFormat);
    }
}
