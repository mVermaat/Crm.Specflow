using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    class FormState
    {
        public bool? Visible { get; set; }
        public bool? Locked { get; set; }
        public RequiredState? Required { get; set; }

    }
}
