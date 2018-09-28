using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Extensions
{
    public static class LoginExtensions
    {
        /// <summary>
        /// Login Page
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <param name="username">The Username to login to CRM application</param>
        /// <param name="password">The Password to login to CRM application</param>
        /// <param name="redirectAction">The RedirectAction</param>
        /// <example>xrmBrowser.LoginPage.Login(_xrmUri, _username, _password, ADFSLogin);</example>
        public static BrowserCommandResult<LoginResult> CustomCZLogin(this LoginDialog login, Uri uri, SecureString username, SecureString password)
        {
            return login.Execute(BrowserOptionHelper.GetOptions("Custom CZ login"), CustomCZLogin, uri, username, password, default(Action<LoginRedirectEventArgs>));
        }

        private static LoginResult CustomCZLogin(IWebDriver driver, Uri uri, SecureString username, SecureString password, Action<LoginRedirectEventArgs> redirectAction)
        {
            driver.Navigate().GoToUrl(uri);

            driver.WaitUntilAvailable(By.XPath("id(\"AuthSignOutLink\")"), TimeSpan.FromSeconds(10), "Signoutlink was not found").Click();
            driver.WaitUntilAvailable(By.Id("otherTileText"), TimeSpan.FromSeconds(20), "Use other account link not found").Click();

            driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]),
                $"The Office 365 sign in page did not return the expected result and the user '{username}' could not be signed in.");

            driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(username.ToUnsecureString());
            driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(Keys.Tab);
            driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.UserId])).SendKeys(Keys.Enter);

            Thread.Sleep(1000);

            //Check if account selection screen is present (AAD vs MSA accounts)
            if (driver.IsVisible(By.Id("aadTile")))
            {
                driver.FindElement(By.Id("aadTile")).Click(true);
            }

            Thread.Sleep(1000);


            driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(password.ToUnsecureString());
            driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).SendKeys(Keys.Tab);
            driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword])).Submit();

            if (driver.IsVisible(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])))
            {
                driver.ClickWhenAvailable(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]));
                driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.StaySignedIn])).Submit();
            }

            driver.WaitUntilVisible(By.XPath(Elements.Xpath[Reference.Login.CrmMainPage])
                , new TimeSpan(0, 0, 60),
                e =>
                {
                    e.WaitForPageToLoad();
                    e.SwitchTo().Frame(0);
                    e.WaitForPageToLoad();
                },
                f => { throw new Exception("Login page failed."); });

            return LoginResult.Success;
        }
    }
}
