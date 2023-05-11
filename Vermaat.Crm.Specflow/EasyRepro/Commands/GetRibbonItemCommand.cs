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
        private readonly string _flyoutParentButtonName;

        public GetRibbonItemCommand(string buttonName)
        {
            if(string.IsNullOrEmpty(buttonName))
                throw new ArgumentNullException(nameof(buttonName));

            if (buttonName.Contains('.'))
            {
                _flyoutParentButtonName = buttonName.Substring(0, buttonName.IndexOf('.'));
                _subButtonName = buttonName.Substring(buttonName.IndexOf('.') + 1);
            }
            else
            {
                _subButtonName = buttonName;
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

            if(string.IsNullOrEmpty(_flyoutParentButtonName))
            {
                return GetRibbonButton(browserInteraction, _subButtonName, ribbon);
            }
            {
                var result = GetRibbonButton(browserInteraction, _flyoutParentButtonName, ribbon);
                if (!result.IsSuccessfull)
                    return result;

                if (result.Result.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Button, _subButtonName), out var subItem))
                    return CommandResult<IWebElement>.Success(subItem);
                else
                    return CommandResult<IWebElement>.Success(null);
            }


        }

        private CommandResult<IWebElement> GetRibbonButton(BrowserInteraction browserInteraction, string buttonName, IWebElement ribbon)
        {
            // Find in regular buttons, return if found
            if (ribbon.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Button, buttonName), out var item))
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
                if (ribbon.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Button, buttonName), out item))
                    return CommandResult<IWebElement>.Success(item);
            }

            // Item isn't in the ribbon
            return CommandResult<IWebElement>.Success(null);
        }
    }
}
