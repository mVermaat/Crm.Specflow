using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    [Binding]
    public class BrowserHooks
    {
        private readonly SeleniumTestingContext _seleniumTestingContext;

        public BrowserHooks(SeleniumTestingContext seleniumTestingContext)
        {
            _seleniumTestingContext = seleniumTestingContext;
        }

        [BeforeScenario("Chrome")]
        public void ChromeSetup()
        {
            _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Chrome;
        }

        [BeforeScenario("Edge")]
        public void EdgeSetup()
        {
            _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Edge;
        }

        [BeforeScenario("Firefox")]
        public void FirefoxSetup()
        {
            _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Firefox;
        }

        [BeforeScenario("IE")]
        public void IESetup()
        {
            _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.IE;
        }
    }
}
