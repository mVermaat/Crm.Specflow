using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class BooleanValue
    {
        public BooleanValue(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public string TextValue => Value.ToString(CultureInfo.InvariantCulture);

        public BooleanItem ToBooleanItem(string logicalName)
        {
            return new BooleanItem { Name = logicalName, Value = Value };
        }
    }
}
