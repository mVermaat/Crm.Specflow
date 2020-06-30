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
        private readonly SeleniumExecutor _executor;
        private readonly WebClient _client;

        public CELogin(SeleniumExecutor executor, WebClient client)
        {
            _executor = executor;
            _client = client;
        }

        public void Login(ICrmConnection connection)
        {
            _executor.Execute("Login to CE", (driver, selectors) =>
            {
                bool online = !(_client.OnlineDomains != null && !_client.OnlineDomains.Any(d => GlobalContext.Url.Host.EndsWith(d)));
                driver.Navigate().GoToUrl(GlobalContext.Url);

                if (!online)
                    return LoginResult.Success;

                driver.WaitUntilClickable(By.Id("use_another_account_link"), TimeSpan.FromSeconds(1), e => e.Click());

                bool success = EnterUserName(driver, connection.BrowserLoginDetails.Username);
                driver.ClickIfVisible(By.Id("aadTile"));
                _client.Browser.ThinkTime(1000);

                EnterPassword(driver, connection.BrowserLoginDetails.Password);
                _client.Browser.ThinkTime(1000);

                int attempts = 0;
                do
                {
                    success = ClickStaySignedIn(driver);
                    attempts++;
                }
                while (!success && attempts <= 3);

                if (success)
                    WaitForMainPage(driver);
                else
                    throw new InvalidOperationException("Login failed");

                return success ? LoginResult.Success : LoginResult.Failure;
            });
        }

        private bool EnterUserName(IWebDriver driver, string username)
        {
            var input = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]), new TimeSpan(0, 0, 30));
            if (input == null)
                return false;

            input.SendKeys(username);
            input.SendKeys(Keys.Enter);
            return true;
        }

        private void EnterPassword(IWebDriver driver, SecureString password)
        {
            var input = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.LoginPassword]), new TimeSpan(0, 0, 30));
            input.SendKeys(password.ToUnsecureString());
            input.Submit();
        }

        private bool ClickStaySignedIn(IWebDriver driver)
        {
            var xpath = By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]);
            var element = driver.ClickIfVisible(xpath, 5.Seconds());
            return element != null;
        }

        private bool WaitForMainPage(IWebDriver driver)
        {
            Action<IWebElement> successCallback = _ =>
            {
                bool isUCI = driver.HasElement(By.XPath(Elements.Xpath[Reference.Login.CrmUCIMainPage]));
                if (isUCI)
                    driver.WaitForTransaction();
            };

            var xpathToMainPage = By.XPath(Elements.Xpath[Reference.Login.CrmMainPage]);
            var element = driver.WaitUntilAvailable(xpathToMainPage, TimeSpan.FromSeconds(30), successCallback);
            return element != null;
        }
    }
}
