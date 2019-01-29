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

        [BeforeScenario("API")]
        public void APISetup()
        {

        }

        [BeforeScenario("Chrome")]
        public void ChromeSetup()
        {
            if (ScenarioContext.Current.IsTagTargetted("Chrome"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Chrome;
                Login();
            }
        }

        [BeforeScenario("Edge")]
        public void EdgeSetup()
        {
            if (ScenarioContext.Current.IsTagTargetted("Edge"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Edge;
                Login();
            }
        }

        [BeforeScenario("Firefox")]
        public void FirefoxSetup()
        {
            if (ScenarioContext.Current.IsTagTargetted("Firefox"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Firefox;
                Login();
            }
        }

        [BeforeScenario("IE")]
        public void IESetup()
        {
            if (ScenarioContext.Current.IsTagTargetted("IE"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.IE;
                Login();
            }
        }


        private void Login()
        {
            _seleniumTestingContext.Browser.LoginPage.Login(new Uri(
                _crmContext.ConnectionInfo.Url),
                _crmContext.ConnectionInfo.Username.ToSecureString(),
                _crmContext.ConnectionInfo.Password.ToSecureString());
        }

       
    }
}
