using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.CommonModels;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class GetRibbonItemCommand : ISeleniumCommandFunc<IWebElement>
    {
        private readonly string _subButtonName;
        private readonly string _mainItemName;

        public GetRibbonItemCommand(string buttonName)
        {
            if(string.IsNullOrEmpty(buttonName))
                throw new ArgumentNullException(nameof(buttonName));

            if (buttonName.Contains('.'))
            {
                _mainItemName = buttonName.Substring(0, buttonName.IndexOf('.'));
                _subButtonName = buttonName.Substring(buttonName.IndexOf('.') + 1);
            }
            else
            {
                _mainItemName = buttonName;
            }
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


            if (ribbon.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Button, _mainItemName), out var mainBarItem))
            {
                if (string.IsNullOrEmpty(_subButtonName))
                    return CommandResult<IWebElement>.Success(mainBarItem);
                else
                    return GetFlyoutSubButton(browserInteraction, mainBarItem);

            }
            else
            {
                // Not found, look for more commands button
                if (ribbon.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_More_Commands), out var moreCommands))
                {
                    moreCommands.Click();

                    // Find the ribbon in the flyout
                    var flyout = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Flyout_Container), TimeSpan.FromSeconds(5));
                    if (flyout == null)
                        return CommandResult<IWebElement>.Fail(true, Constants.ErrorCodes.RIBBON_NOT_FOUND, "more commands");

                    // Find in more commands list
                    if (flyout.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Button, _mainItemName), out var moreCommandsItem))
                    {
                        if (string.IsNullOrEmpty(_subButtonName))
                            return CommandResult<IWebElement>.Success(moreCommandsItem);
                        else
                            return GetFlyoutSubButton(browserInteraction, moreCommandsItem);
                    }
                }

                // Item isn't in the ribbon
                return CommandResult<IWebElement>.Success(null);
            }
        }

        private CommandResult<IWebElement> GetFlyoutSubButton(BrowserInteraction browserInteraction, IWebElement mainButtonItem)
        {
            mainButtonItem.Click();
            var flyout = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.FlyoutRoot), TimeSpan.FromSeconds(5));
            if (flyout == null)
                return CommandResult<IWebElement>.Fail(true, Constants.ErrorCodes.RIBBON_NOT_FOUND, $"flyout from {_mainItemName}");

            if (flyout.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Button, _subButtonName), out var subCommand))
                return CommandResult<IWebElement>.Success(subCommand);
            else
                return CommandResult<IWebElement>.Success(null);
        }

    }
}
