using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Processors;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    [Binding]
    public class BrowserHooks
    {
        private readonly SeleniumTestingContext _seleniumTestingContext;
        private readonly CrmTestingContext _crmContext;
        private readonly StepProcessor _processor;

        public BrowserHooks(SeleniumTestingContext seleniumTestingContext, CrmTestingContext crmContext, StepProcessor processor)
        {
            _seleniumTestingContext = seleniumTestingContext;
            _crmContext = crmContext;
            _processor = processor;
        }

        [BeforeScenario("Chrome")]
        public void ChromeSetup()
        {
            if (ScenarioContext.Current.IsTagTargetted("Chrome"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Chrome;
                SetupProcessor();
                Login();
            }
        }

        [BeforeScenario("Edge")]
        public void EdgeSetup()
        {
            if (ScenarioContext.Current.IsTagTargetted("Edge"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Edge;
                SetupProcessor();
                Login();
            }
        }

        [BeforeScenario("Firefox")]
        public void FirefoxSetup()
        {
            if (ScenarioContext.Current.IsTagTargetted("Firefox"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Chrome;
                SetupProcessor();
                Login();
            }
        }

        [BeforeScenario("IE")]
        public void IESetup()
        {
            if (ScenarioContext.Current.IsTagTargetted("IE"))
            {
                _seleniumTestingContext.BrowserOptions.BrowserType = BrowserType.Chrome;
                SetupProcessor();
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

        private void SetupProcessor()
        {
            _processor.GeneralCrm = new CrmStepUIProcessor(_crmContext, _seleniumTestingContext);
            _processor.SetDefaultProcessors(_crmContext, false);
        }
    }
}
