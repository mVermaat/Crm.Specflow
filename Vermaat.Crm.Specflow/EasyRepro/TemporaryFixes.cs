using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public static class TemporaryFixes
    {

        #region https://github.com/DynamicHands/Crm.Specflow/issues/44

        /// <summary>
        /// Sets the value of a Date Field.
        /// </summary>
        /// <param name="field">Date field name.</param>
        /// <param name="value">DateTime value.</param>
        /// <param name="formatDate">Datetime format matching Short Date formatting personal options.</param>
        /// <param name="formatTime">Datetime format matching Short Time formatting personal options.</param>
        /// <example>xrmApp.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        public static BrowserCommandResult<bool> SetValueFix(this WebClient client, string field, DateTime? value, string formatDate = null, string formatTime = null)
        {
            return client.Execute(BrowserOptionHelper.GetOptions($"Set Date/Time Value: {field}"), driver =>
            {
                driver.WaitForTransaction();
                var xPath = By.XPath(AppElements.Xpath[AppReference.Entity.FieldControlDateTimeInputUCI].Replace("[FIELD]", field));

                var dateField = driver.WaitUntilAvailable(xPath, $"Field: {field} Does not exist");
                try
                {
                    var date = value.HasValue ? formatDate == null ? value.Value.ToShortDateString() : value.Value.ToString(formatDate) : string.Empty;
                    driver.RepeatUntil(() =>
                    {
                        ClearFieldValue(dateField, client.Browser);
                        dateField.SendKeys(date);
                    },
                        d => dateField.GetAttribute("value") == date,
                        new TimeSpan(0, 0, 9), 3
                    );
                    driver.WaitForTransaction();
                }
                catch (WebDriverTimeoutException ex)
                {
                    throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {value}. Actual: {dateField.GetAttribute("value")}", ex);
                }
                // Try Set Time
                var timeFieldXPath = By.XPath($"//div[contains(@data-id,'{field}.fieldControl._timecontrol-datetime-container')]/div/div/input");
                bool success = driver.TryFindElement(timeFieldXPath, out var timeField);
                if (!success || timeField == null)
                    return true;
                try
                {
                    var time = value.HasValue ? formatTime == null ? value.Value.ToShortTimeString() : value.Value.ToString(formatTime) : string.Empty;
                    driver.RepeatUntil(() =>
                    {
                        ClearFieldValue(timeField, client.Browser);
                        timeField.SendKeys(time + Keys.Tab);
                    },
                        d => timeField.GetAttribute("value") == time,
                        new TimeSpan(0, 0, 9), 3
                    );
                    driver.WaitForTransaction();

                }
                catch (WebDriverTimeoutException ex)
                {
                    throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {value}. Actual: {timeField.GetAttribute("value")}", ex);
                }

                return true;
            });
        }

        private static void ClearFieldValue(IWebElement field, InteractiveBrowser browser)
        {
            if (field.GetAttribute("value").Length > 0)
            {
                field.SendKeys(Keys.Control + "a");
                field.SendKeys(Keys.Backspace);
            }
            browser.ThinkTime(2000);
        }

        public static bool RepeatUntil(this IWebDriver driver, Action action, Predicate<IWebDriver> predicate,
                                       TimeSpan? timeout = null,
                                       int attemps = 2,
                                       Action successCallback = null, Action failureCallback = null)
        {
            timeout = timeout ?? new TimeSpan(0,0,30);
            var waittime = new TimeSpan(timeout.Value.Ticks / attemps);

            WebDriverWait wait = new WebDriverWait(driver, waittime);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            bool success = predicate(driver);
            while (!success && attemps > 0)
            {
                try
                {
                    action();
                    attemps--;
                    success = wait.Until(d => predicate(d));
                }
                catch (WebDriverTimeoutException) { }
            }

            if (success)
                successCallback?.Invoke();
            else
                failureCallback?.Invoke();

            return success;
        }

        private static IWebElement WaitUntilClickable(IWebDriver driver, By by, TimeSpan timeout, Action<IWebDriver> successCallback, Action<IWebDriver> failureCallback)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            bool? success;
            IWebElement returnElement = null;

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            try
            {
                returnElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
                success = true;
            }
            catch (NoSuchElementException)
            {
                success = false;
            }
            catch (WebDriverTimeoutException)
            {
                success = false;
            }

            if (success.HasValue && success.Value && successCallback != null)
                successCallback(driver);
            else if (success.HasValue && !success.Value && failureCallback != null)
                failureCallback(driver);

            return returnElement;
        }


        #endregion

        #region https://github.com/DynamicHands/Crm.Specflow/issues/78

        public static void Login(WebClient client, Uri orgUri, SecureString username, SecureString password)
        {
            client.Execute(BrowserOptionHelper.GetOptions("Login"), Login, client, orgUri, username, password); 
        }

        private static LoginResult Login(IWebDriver driver, WebClient client, Uri uri, SecureString username, SecureString password)
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

        private static bool EnterUserName(IWebDriver driver, SecureString username)
        {
            var input = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]), new TimeSpan(0, 0, 30));
            if (input == null)
                return false;

            input.SendKeys(username.ToUnsecureString());
            input.SendKeys(Keys.Enter);
            return true;
        }

        private static void EnterPassword(IWebDriver driver, SecureString password)
        {
            var input = driver.FindElement(By.XPath(Elements.Xpath[Reference.Login.LoginPassword]));
            input.SendKeys(password.ToUnsecureString());
            input.Submit();
        }

        private static bool ClickStaySignedIn(IWebDriver driver)
        {
            var xpath = By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]);
            var element = driver.ClickIfVisible(xpath, 5.Seconds());
            return element != null;
        }

        internal static bool WaitForMainPage(IWebDriver driver)
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

        #endregion

        #region https://github.com/DynamicHands/Crm.Specflow/issues/88

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.Entity.SetValue("firstname", "Test");</example>
        public static BrowserCommandResult<bool> SetValueFix(this WebClient client, string field, string value)
        {
            return client.Execute(BrowserOptionHelper.GetOptions("Set Value"), driver =>
            {

                IWebElement fieldContainer = null;

                // Initialize the entity form context
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.FormContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));


                IWebElement input;
                bool found = fieldContainer.TryFindElement(By.TagName("input"), out input);

                if (!found)
                    found = fieldContainer.TryFindElement(By.TagName("textarea"), out input);

                if (!found)
                    throw new NoSuchElementException($"Field with name {field} does not exist.");

                SetInputValue(driver, input, value);

                return true;
            });
        }

        private static void SetInputValue(IWebDriver driver, IWebElement input, string value, TimeSpan? thinktime = null)
        {
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Backspace);
            driver.WaitForTransaction();

            if (string.IsNullOrWhiteSpace(value))
            {
                input.Click(true);
                return;
            }

            input.SendKeys(value, true);
            driver.WaitForTransaction();
        }


        #endregion



    }
}
