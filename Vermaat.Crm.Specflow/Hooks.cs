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
        private readonly SeleniumTestingContext _seleniumTestingContext;
        private readonly CrmTestingContext _crmContext;

        public Hooks(SeleniumTestingContext seleniumTestingContext, CrmTestingContext crmContext)
        {
            _seleniumTestingContext = seleniumTestingContext;
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
            if (HelperMethods.IsTagTargetted("Chrome"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Chrome;
                _seleniumTestingContext.Browser.Login(_crmContext.ConnectionInfo);
            }
        }

        [BeforeScenario("Edge")]
        public void EdgeSetup()
        {
            if (HelperMethods.IsTagTargetted("Edge"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Edge;
                _seleniumTestingContext.Browser.Login(_crmContext.ConnectionInfo);
            }
        }

        [BeforeScenario("Firefox")]
        public void FirefoxSetup()
        {
            if (HelperMethods.IsTagTargetted("Firefox"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Firefox;
                _seleniumTestingContext.Browser.Login(_crmContext.ConnectionInfo);
            }
        }

        [BeforeScenario("IE")]
        public void IESetup()
        {
            if (HelperMethods.IsTagTargetted("IE"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.IE;
                _seleniumTestingContext.Browser.Login(_crmContext.ConnectionInfo);
            }
        }     
    }
}
