using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormSection : FormComponent
    {
        public string Label { get; set; }
        public bool Visible { get; set; }

        public override bool IsVisible()
        {
            return Visible;
        }
    }
}
