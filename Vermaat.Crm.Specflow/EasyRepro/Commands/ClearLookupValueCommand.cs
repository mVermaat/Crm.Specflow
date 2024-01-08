using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class ClearLookupValueCommand : ISeleniumCommand
    {
        private readonly LookupItem _lookupItem;

        public ClearLookupValueCommand(LookupItem lookupItem)
        {
            _lookupItem = lookupItem;
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            var lookupItemContainer = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_LookupItemContainer, _lookupItem.Name), TimeSpan.FromSeconds(2));
            
            if(!lookupItemContainer.TryFindElement(By.TagName("ul"), out var lookupItemList)) 
            {
                Logger.WriteLine("Lookup is already empty");
                return CommandResult.Success();
            }
            var listItems = lookupItemList.FindElements(By.TagName("li"));
            Logger.WriteLine($"Lookup has {listItems.Count} values");
            foreach(var item in listItems)
            {
                item.Hover(browserInteraction.Driver);
                var deleteButton = item.WaitUntilClickable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_LookupDeleteItem, _lookupItem.Name), TimeSpan.FromSeconds(2));

                if (deleteButton == null)
                { // workaround as sometimes the delete button doesn't show up properly. Need to figure out why.
                    browserInteraction.Driver.ExecuteScript($"Xrm.Page.getAttribute('{_lookupItem.Name}').setValue(null)");
                    browserInteraction.Driver.ExecuteScript($"Xrm.Page.getAttribute('{_lookupItem.Name}').fireOnChange()");
                    return CommandResult.Success();
                }
                deleteButton.Click();
            }

            return CommandResult.Success();
        }
    }
}
