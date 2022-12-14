using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Entities
{
    public enum EnvironmentVariableType
    {
        String = 100000000,
        Number = 100000001,
        Boolean = 100000002,
        JSON = 100000003,
        DataSource = 100000004,
        Secret = 100000005,
    }
}
