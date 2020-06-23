using Microsoft.Dynamics365.UIAutomation.Browser;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Hooks
{
    [Binding]
    public class BrowserHooks
    {
        private readonly ICrmContext _crmContext;
        private readonly ISeleniumContext _seleniumContext;

        public BrowserHooks(ICrmContext crmContext, ISeleniumContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        [BeforeScenario("Chrome")]
        public void ChromeSetup()
        {
            if (_crmContext.IsTarget("Chrome"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.Chrome;
            }
        }

        [BeforeScenario("Edge")]
        public void EdgeSetup()
        {
            if (_crmContext.IsTarget("Edge"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.Edge;
            }
        }

        [BeforeScenario("Firefox")]
        public void FirefoxSetup()
        {
            if (_crmContext.IsTarget("Firefox"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.Firefox;
            }
        }

        [BeforeScenario("IE")]
        public void IESetup()
        {
            if (_crmContext.IsTarget("IE"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.IE;
            }
        }
    }
}
