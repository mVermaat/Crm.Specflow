using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.FormLoadConditions
{
    class NoBusinessProcessError : IFormLoadCondition
    {
        public bool Evaluate(IWebDriver driver, SeleniumSelectorData selectors)
        {
            if(driver.HasElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_ErrorDialog)))
            {
                var subtitle = driver.TryFindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Subtitle), out IWebElement subTitle);
                throw new TestExecutionException(Constants.ErrorCodes.BUSINESS_PROCESS_ERROR_WHEN_LOADING, subTitle?.Text ?? "No message");
            }
            else
            {
                return true;
            }
        }
    }
}
