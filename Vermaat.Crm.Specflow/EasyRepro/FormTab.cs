using System.Collections.Generic;
using System.Windows.Forms;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormTab : FormComponent
    {
        public string Label { get; set; }
        public bool Visible { get; set; }
        public Dictionary<string, FormSection> Sections { get; private set; }

        public FormTab(Dictionary<string, FormSection> sections)
        {
            Sections = sections;
        }

        public override bool IsVisible()
        {
            return Visible;
        }
    }
}
