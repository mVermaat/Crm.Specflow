using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public partial class ButtonTexts
    {
        public ButtonTexts(bool setDefaults = true)
        {
            if (setDefaults)
            {
                SaveAndClose = "Save & Close";
                New = "New";
                Delete = "Delete";
                Save = "Save";
                ActivateQuote = "Activate Quote";
                CreateOrder = "Create Order";
                ReviseQuote = "Revise";
                CloseQuote = "Close Quote";
            }
        }

        public string SaveAndClose { get; set; }
        public string New { get; set; }
        public string Delete { get; set; }
        public string Save { get; set; }
        public string ActivateQuote { get; set; }
        public string CreateOrder { get; set; }
        public string ReviseQuote { get; set; }
        public string CloseQuote { get; set; }
    }
}
