using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow
{
    [Binding]
    public class Hooks
    {
        private readonly SeleniumTestingContext _seleniumContext;
        private readonly CrmTestingContext _crmContext;

        public Hooks(SeleniumTestingContext seleniumTestingContext, CrmTestingContext crmContext)
        {
            _seleniumContext = seleniumTestingContext;
            _crmContext = crmContext;
        }

        [AfterScenario("Cleanup")]
        public void Cleanup()
        {
            _crmContext.RecordCache.DeleteAllCachedRecords(_crmContext.Service);
        }

        [BeforeScenario("API")]
        public void APISetup()
        {

        }

        [BeforeScenario("Chrome")]
        public void ChromeSetup()
        {
            if (_seleniumContext.IsTarget("Chrome"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.Chrome;
            }
        }

        [BeforeScenario("Edge")]
        public void EdgeSetup()
        {
            if (_seleniumContext.IsTarget("Edge"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.Edge;
            }
        }

        [BeforeScenario("Firefox")]
        public void FirefoxSetup()
        {
            if (_seleniumContext.IsTarget("Firefox"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.Firefox;
            }
        }

        [BeforeScenario("IE")]
        public void IESetup()
        {
            if (_seleniumContext.IsTarget("IE"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.IE;
            }
        }     
    }
}
