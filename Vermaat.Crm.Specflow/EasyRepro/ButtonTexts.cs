using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class ButtonTexts
    {
        public ButtonTexts(bool setDefaults = true)
        {
            if (setDefaults)
            {
                SaveAndClose = "Save & Close";
                New = "New";
                Delete = "Delete";
            }
        }

        public string SaveAndClose { get; set; }
        public string New { get; set; }
        public string Delete { get; set; }

    }
}
