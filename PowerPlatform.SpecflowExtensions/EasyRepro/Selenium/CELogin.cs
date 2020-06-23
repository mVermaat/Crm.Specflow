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
    class CELogin : ILogin
    {
        private readonly WebClient _client;

        public CELogin(WebClient client)
        {
            _client = client;
        }

        public void Login(ICrmConnection connection)
        {
            _client.Execute(BrowserOptionHelper.GetOptions("Login"), LoginToCE, _client, connection.Url, 
                connection.BrowserLoginDetails.Username.ToSecureString(),
                connection.BrowserLoginDetails.Password);
        }

        private LoginResult LoginToCE(IWebDriver driver, WebClient client, Uri uri, SecureString username, SecureString password)
        {
            bool online = !(client.OnlineDomains != null && !client.OnlineDomains.Any(d => uri.Host.EndsWith(d)));
            driver.Navigate().GoToUrl(uri);

            if (!online)
                return LoginResult.Success;

            driver.WaitUntilClickable(By.Id("use_another_account_link"), TimeSpan.FromSeconds(1), e => e.Click());

            bool success = EnterUserName(driver, username);
            driver.ClickIfVisible(By.Id("aadTile"));
            client.Browser.ThinkTime(1000);

            EnterPassword(driver, password);
            client.Browser.ThinkTime(1000);

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
        }

        private bool EnterUserName(IWebDriver driver, SecureString username)
        {
            var input = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]), new TimeSpan(0, 0, 30));
            if (input == null)
                return false;

            input.SendKeys(username.ToUnsecureString());
            input.SendKeys(Keys.Enter);
            return true;
        }

        private void EnterPassword(IWebDriver driver, SecureString password)
        {
            var input = driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword]));
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
