using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class DoubleValue
    {
        public DoubleValue(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public string TextValue => Value.ToString(
            GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.NumberFormat);
    }
}
