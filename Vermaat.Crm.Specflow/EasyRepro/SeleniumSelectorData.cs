using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    internal class SeleniumSelectorData
    {
        private Dictionary<SeleniumSelectorItems, string> selectors = new Dictionary<SeleniumSelectorItems, string>()
        {
            { SeleniumSelectorItems.Entity_SubGrid, "//div[@id=\"dataSetRoot_[NAME]\"]" },
            { SeleniumSelectorItems.Entity_SubGrid_ButtonList, ".//ul[@data-id='CommandBar']" },
            { SeleniumSelectorItems.Entity_SubGrid_Button, ".//button[contains(@data-id,'[NAME]')]" }
        };

        public By GetXPathSeleniumSelector(SeleniumSelectorItems itemName)
        {
            return By.XPath(selectors[itemName]);
        }

        public By GetXPathSeleniumSelector(SeleniumSelectorItems itemName, string nameReplacement)
        {
            return By.XPath(selectors[itemName].Replace("[NAME]", nameReplacement));
        }
    }
}
