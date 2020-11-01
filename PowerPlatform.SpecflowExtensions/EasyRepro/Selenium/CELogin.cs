using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using PowerPlatform.SpecflowExtensions.Connectivity;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    internal class CELogin : ILogin
    {
        private readonly ISeleniumExecutor _executor;
        private readonly WebClient _client;

        public CELogin(ISeleniumExecutor executor, WebClient client)
        {
            _executor = executor;
            _client = client;
        }

        public void Login(ICrmConnection connection)
        {
            _executor.Execute("Login to CE", (driver, selectors) =>
            {
                return TemporaryFixes.Login(driver, _client, connection);
            });
        }
    }
}
