using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.FormLoadConditions;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class ConvertActiveQuoteToSalesOrderCommand : ISeleniumCommand
    {
        public ConvertActiveQuoteToSalesOrderCommand()
        {
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            var container = browserInteraction.Driver.WaitUntilAvailable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Container));
            var button = container.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_OK));

            button.Click();
            HelperMethods.WaitForFormLoad(browserInteraction.Driver, new FormIsOfEntity("salesorder"));
            return CommandResult.Success();

        }
    }
}
