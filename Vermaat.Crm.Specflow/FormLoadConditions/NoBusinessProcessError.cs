using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.FormLoadConditions
{
    class NoBusinessProcessError : IFormLoadCondition
    {
        public bool Evaluate(IWebDriver driver)
        {
            if(driver.HasElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_ErrorDialog)))
            {
                var error = SeleniumFunctions.GetErrorDialogMessage(driver);
                throw new TestExecutionException(Constants.ErrorCodes.BUSINESS_PROCESS_ERROR_WHEN_LOADING, error);
            }
            else
            {
                return true;
            }
        }
    }
}
