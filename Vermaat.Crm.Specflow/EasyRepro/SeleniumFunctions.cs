using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    internal static class SeleniumFunctions
    {
        private static SeleniumSelectorData _xpathData = new SeleniumSelectorData();

       
        public static void ClickSubgridButton(this WebClient client, string subgridName, string subgridButtonId)
        {
            client.Execute(BrowserOptionHelper.GetOptions($"Set Value"), driver =>
            {
                var subGrid = driver.FindElement(_xpathData.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SubGrid, subgridName));
                var menuBar = subGrid.FindElement(_xpathData.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SubGrid_ButtonList));

                var buttons = menuBar.FindElements(By.TagName("button"));

                var button = buttons.FirstOrDefault(b => b.GetAttribute("data-id").Contains(subgridButtonId));
                if(button != null)
                {
                    button.Click();
                    return true;
                }

                var moreCommands = buttons.FirstOrDefault(b => b.GetAttribute("data-id").Equals("OverflowButton"));
                if (moreCommands == null)
                    throw new InvalidOperationException("More commands button not found");
                moreCommands.Click();

                var flyout = driver.FindElement(By.Id("__flyoutRootNode"));
                flyout.FindElement(_xpathData.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SubGrid_Button, subgridButtonId)).Click();

                return true;
            });
        }
    }
}
