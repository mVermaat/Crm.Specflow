using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class GetRibbonItemCommand : ISeleniumCommandFunc<IWebElement>
    {
        private readonly string _buttonName;

        public GetRibbonItemCommand(string buttonName)
        {
            _buttonName = buttonName;
        }

        public virtual CommandResult<IWebElement> Execute(BrowserInteraction browserInteraction)
        {
            // Get ribbon
            var ribbon = browserInteraction.Driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container]),
                    TimeSpan.FromSeconds(5));

            // Get ribbon (alternative)
            if (ribbon == null)
            {
                ribbon = browserInteraction.Driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid]));
                if (ribbon == null)
                    return CommandResult<IWebElement>.Fail(true, Constants.ErrorCodes.RIBBON_NOT_FOUND, "main");
            }

            // Find in regular buttons, return if found
            if (ribbon.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Button, _buttonName), out var item))
                return CommandResult<IWebElement>.Success(item);

            // Not found, look for more commands button
            if(ribbon.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_More_Commands), out var moreCommands))
            {
                moreCommands.Click();
                
                // Find the ribbon in the flyout
                ribbon = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Flyout_Container), TimeSpan.FromSeconds(5));
                if (ribbon == null)
                    return CommandResult<IWebElement>.Fail(true, Constants.ErrorCodes.RIBBON_NOT_FOUND, "more commands");


                // Find in more commands list
                if (ribbon.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Button, _buttonName), out item))
                    return CommandResult<IWebElement>.Success(item);
            }

            // Item isn't in the ribbon
            return CommandResult<IWebElement>.Success(null);

        }        
    }
}
