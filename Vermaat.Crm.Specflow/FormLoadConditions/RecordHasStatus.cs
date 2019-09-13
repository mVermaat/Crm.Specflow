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
    public class RecordHasStatus : IFormLoadCondition
    {
        private readonly string _status;

        public RecordHasStatus(string status)
        {
            _status = status;
        }

        public bool Evaluate(IWebDriver driver)
        {
            Logger.WriteLine($"Evaluating if current record's footer status is {_status}");
            var footerStatus = driver.WaitUntilAvailable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Footer_Status), TimeSpan.FromSeconds(5));

            if (footerStatus == null)
                return false;

            var value = footerStatus.Text;

            return !string.IsNullOrEmpty(value) && value.Equals(_status, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
