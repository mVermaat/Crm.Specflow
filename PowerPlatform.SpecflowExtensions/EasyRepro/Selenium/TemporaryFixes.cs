using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PowerPlatform.SpecflowExtensions.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    internal static class TemporaryFixes
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
        public static void SetDateTimeValue(IWebDriver driver, string field, DateTime? value, string formatDate = null, string formatTime = null)
        {
            driver.WaitForTransaction();
            var xPath = By.XPath(AppElements.Xpath[AppReference.Entity.FieldControlDateTimeInputUCI].Replace("[FIELD]", field));

            var dateField = driver.WaitUntilAvailable(xPath, $"Field: {field} Does not exist");
            try
            {
                var date = value.HasValue ? formatDate == null ? value.Value.ToShortDateString() : value.Value.ToString(formatDate) : string.Empty;
                driver.RepeatUntil(() =>
                {
                    ClearFieldValue(dateField);
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
                return;
            try
            {
                var time = value.HasValue ? formatTime == null ? value.Value.ToShortTimeString() : value.Value.ToString(formatTime) : string.Empty;
                driver.RepeatUntil(() =>
                {
                    ClearFieldValue(timeField);
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
        }

        private static void ClearFieldValue(IWebElement field)
        {
            if (field.GetAttribute("value").Length > 0)
            {
                field.SendKeys(Keys.Control + "a");
                field.SendKeys(Keys.Backspace);
            }
            Thread.Sleep(1000);
        }

        private static bool RepeatUntil(this IWebDriver driver, Action action, Predicate<IWebDriver> predicate,
                                       TimeSpan? timeout = null,
                                       int attemps = 2,
                                       Action successCallback = null, Action failureCallback = null)
        {
            timeout = timeout ?? new TimeSpan(0, 0, 30);
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

        public static LoginResult Login(IWebDriver driver, WebClient client, ICrmConnection connection)
        {
                bool online = !(client.OnlineDomains != null && !client.OnlineDomains.Any(d => GlobalContext.Url.Host.EndsWith(d)));
                driver.Navigate().GoToUrl(GlobalContext.Url);

                if (!online)
                    return LoginResult.Success;

                driver.WaitUntilClickable(By.Id("use_another_account_link"), TimeSpan.FromSeconds(1), e => e.Click());

                bool success = EnterUserName(driver, connection.BrowserLoginDetails.Username);
                driver.ClickIfVisible(By.Id("aadTile"));
                client.Browser.ThinkTime(1000);

                EnterPassword(driver, connection.BrowserLoginDetails.Password);
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

        private static bool EnterUserName(IWebDriver driver, string username)
        {
            var input = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.UserId]), new TimeSpan(0, 0, 30));
            if (input == null)
                return false;

            input.SendKeys(username);
            input.SendKeys(Keys.Enter);
            return true;
        }

        private static void EnterPassword(IWebDriver driver, SecureString password)
        {
            var input = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Login.LoginPassword]), new TimeSpan(0, 0, 30));
            input.SendKeys(password.ToUnsecureString());
            input.Submit();
        }

        private static bool ClickStaySignedIn(IWebDriver driver)
        {
            var xpath = By.XPath(Elements.Xpath[Reference.Login.StaySignedIn]);
            var element = driver.ClickIfVisible(xpath, 5.Seconds());
            return element != null;
        }

        private static bool WaitForMainPage(IWebDriver driver)
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

        internal enum ContainerType
        {
            Body, Header, Dialog
        }

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.Entity.SetValue("firstname", "Test");</example>
        public static void SetTextField(IWebDriver driver, SeleniumSelectorData selectors,
            string field, string value, ContainerType formContextType)
        {
            IWebElement fieldContainer = null;

            if (formContextType == ContainerType.Body)
            {
                // Initialize the entity form context
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.FormContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));
            }
            else if (formContextType == ContainerType.Header)
            {
                // Initialize the Header context
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.HeaderContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));
            }
            else if (formContextType == ContainerType.Dialog)
            {
                // Initialize the Dialog context
                var formContext = driver.WaitUntilAvailable(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Container));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));
            }

            bool found = fieldContainer.TryFindElement(By.TagName("input"), out IWebElement input);

            if (!found)
                found = fieldContainer.TryFindElement(By.TagName("textarea"), out input);

            if (!found)
                throw new NoSuchElementException($"Field with name {field} does not exist.");

            SetInputValue(input, value);
        }

        private static void SetInputValue(IWebElement input, string value)
        {
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Backspace);

            if (!string.IsNullOrWhiteSpace(value))
            {
                input.SendKeys(value);
            }

            input.SendKeys(Keys.Tab + Keys.Tab);
        }

        /// <summary>
        /// Sets the value of a picklist or status field.
        /// </summary>
        /// <param name="control">The option you want to set.</param>
        /// <example>xrmApp.Entity.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });</example>
        public static void SetOptionSetValue(IWebDriver driver, SeleniumSelectorData selectors, 
            OptionSet control, ContainerType formContextType)
        {
            var controlName = control.Name;
            
            IWebElement fieldContainer = null;

            if (formContextType == ContainerType.Body)
            {
                // Initialize the entity form context
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.FormContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", controlName)));
            }
            else if (formContextType == ContainerType.Header)
            {
                // Initialize the Header context
                var formContext = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.HeaderContext]));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", controlName)));
            }
            else if (formContextType == ContainerType.Dialog)
            {
                // Initialize the Dialog context
                var formContext = driver.WaitUntilAvailable(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Container));
                fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", controlName)));
            }

            TrySetValue(fieldContainer, control);
        }

        private static void TrySetValue(IWebElement fieldContainer, OptionSet control)
        {
            var value = control.Value;
            bool success = fieldContainer.TryFindElement(By.TagName("select"), out IWebElement select);
            if (success)
            {
                var options = select.FindElements(By.TagName("option"));
                SelectOption(options, value);
                return;
            }

            var name = control.Name;
            var hasStatusCombo = fieldContainer.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusCombo].Replace("[NAME]", name)));
            if (hasStatusCombo)
            {
                // This is for statuscode (type = status) that should act like an optionset doesn't doesn't follow the same pattern when rendered
                fieldContainer.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusComboButton].Replace("[NAME]", name)));

                var listBox = fieldContainer.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.EntityOptionsetStatusComboList].Replace("[NAME]", name)));

                var options = listBox.FindElements(By.TagName("li"));
                SelectOption(options, value);
                return;
            }

            throw new InvalidOperationException($"OptionSet Field: '{name}' does not exist");
        }

        private static void SelectOption(ReadOnlyCollection<IWebElement> options, string value)
        {
            var selectedOption = options.FirstOrDefault(op => op.Text == value || op.GetAttribute("value") == value);
            selectedOption.Click(true);
        }

        #endregion

    }
}
