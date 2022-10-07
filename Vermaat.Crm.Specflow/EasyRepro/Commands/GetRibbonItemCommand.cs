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
    internal class GetRibbonItemCommand : ISeleniumCommandFunc<IWebElement>
    {
        private readonly string _buttonName;

        public GetRibbonItemCommand(string buttonName)
        {
            _buttonName = buttonName;
        }

        public CommandResult<IWebElement> Execute(IWebDriver driver, SeleniumSelectorData selectors)
        {
            var ribbon = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container]),
                    TimeSpan.FromSeconds(5));

            if (ribbon == null)
            {
                ribbon = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid]));
                if (ribbon == null)
                    throw new TestExecutionException(Constants.ErrorCodes.RIBBON_NOT_FOUND);
            }

            var items = ribbon.FindElements(By.TagName("button"));
            var item = FindButton(items, ribbon);
            if (item != null)
                return new CommandResult<IWebElement>() { IsSuccessfull = true, Result = item };

            var moreCommands = items.FirstOrDefault(x => x.GetAttribute("aria-label").Equals("More Commands", StringComparison.OrdinalIgnoreCase));

            if(moreCommands != null)
            {
                moreCommands.Click();
                
                ribbon = driver.WaitUntilAvailable(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Ribbon_Flyout_Container), TimeSpan.FromSeconds(5));
                if(ribbon == null)
                    throw new TestExecutionException(Constants.ErrorCodes.RIBBON_NOT_FOUND);

                items = ribbon.FindElements(By.TagName("button"));
                item = FindButton(items, ribbon);
                if (item != null)
                    return new CommandResult<IWebElement>() { IsSuccessfull = true, Result = item };
            }
            
            return new CommandResult<IWebElement>() { IsSuccessfull = false, AllowRetry = false };

        }

        private IWebElement FindButton(ReadOnlyCollection<IWebElement> items, IWebElement ribbon)
         => items.FirstOrDefault(item => item.GetAttribute("aria-label").Equals(_buttonName, StringComparison.OrdinalIgnoreCase));
        
    }
}
