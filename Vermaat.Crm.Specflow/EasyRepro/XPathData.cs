using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    internal class XPathData
    {
        private Dictionary<XPathItems, string> _xpath = new Dictionary<XPathItems, string>()
        {
            { XPathItems.Entity_SubGrid, "//div[@id=\"dataSetRoot_[NAME]\"]" },
            { XPathItems.Entity_SubGrid_ButtonList, ".//ul[@data-id='CommandBar']" },
            { XPathItems.Entity_SubGrid_Button, ".//button[contains(@data-id,'[NAME]')]" }
        };

        public string GetXPath(XPathItems itemName)
        {
            return _xpath[itemName];
        }

        public string GetXPath(XPathItems itemName, string nameReplacement)
        {
            return _xpath[itemName].Replace("[NAME]", nameReplacement);
        }
    }
}
