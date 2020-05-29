using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class LookupValue
    {
        public LookupValue(EntityReference value)
        {
            Value = value;
        }

        public EntityReference Value { get; }
    }
}
