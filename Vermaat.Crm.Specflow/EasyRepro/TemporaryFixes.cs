using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static BrowserCommandResult<bool> SetValueFix(this WebClient client, string field, DateTime? value, bool dateOnly, string formatDate = null, string formatTime = null)
        {
            return client.Execute(BrowserOptionHelper.GetOptions($"Set Date/Time Value: {field}"), driver =>
            {
                driver.WaitForTransaction();

                var container = driver.WaitUntilAvailable(
                    SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_DateContainer, field),
                    $"Field: {field} does not exist");

                var dateField = container.WaitUntilAvailable(
                    SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_DateTime_Time_Input), 
                    $"Input for {field} does not exist");
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
                }
                catch (WebDriverTimeoutException ex)
                {
                    throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {value}. Actual: {dateField.GetAttribute("value")}", ex);
                }

                // date only fields don't have a time control
                // clearing the date part of a datetime field is enough to clear both
                if (dateOnly || !value.HasValue)
                    return true;

                // Time field becomes visible after focus is lost.
                // Clearfocus will have unwanted side effects like popups or redirects for some reason.
                dateField.SendKeys(Keys.Tab);
                driver.WaitForTransaction();

                var timeFieldXPath = By.XPath($"//div[contains(@data-id,'{field}.fieldControl._timecontrol-datetime-container')]/div/div/input");
                var timeField = driver.WaitUntilAvailable(timeFieldXPath, TimeSpan.FromSeconds(5), "Time control of datetime field not available");
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
                success = ClickStaySignedIn(driver) || WaitForMainPage(driver, 15);
                attempts++;
            }
            while (!success && attempts <= 3);

            if (success)
                WaitForMainPage(driver, 60);
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

        private static bool WaitForMainPage(IWebDriver driver, int timeout)
        {
            Action<IWebElement> successCallback = _ =>
                                  {
                                      bool isUCI = driver.HasElement(By.XPath(Elements.Xpath[Reference.Login.CrmUCIMainPage]));
                                      if (isUCI)
                                          driver.WaitForTransaction();
                                  };

            var xpathToMainPage = By.XPath(Elements.Xpath[Reference.Login.CrmMainPage]);
            var element = driver.WaitUntilAvailable(xpathToMainPage, TimeSpan.FromSeconds(timeout), successCallback);
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
        public static BrowserCommandResult<bool> SetValueFix(this WebClient client, string field, string value, ContainerType formContextType)
        {
            return client.Execute(BrowserOptionHelper.GetOptions("Set Value"), driver =>
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
                    var formContext = driver.WaitUntilAvailable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Container));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));
                }

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

        private static void SetInputValue(IWebDriver driver, IWebElement input, string value)
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
        public static BrowserCommandResult<bool> SetValueFix(this WebClient client, OptionSet control, ContainerType formContextType)
        {
            var controlName = control.Name;
            return client.Execute(BrowserOptionHelper.GetOptions($"Set OptionSet Value: {controlName}"), driver =>
            {
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
                    var formContext = driver.WaitUntilAvailable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Container));
                    fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", controlName)));
                }

                TrySetValue(fieldContainer, control);
                return true;
            });
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

        #region https://github.com/DynamicHands/Crm.Specflow/issues/112

        public static BrowserCommandResult<bool> ClickCommand(this WebClient client, string name, string subname = null, string subSecondName = null)
        {
            return client.Execute(BrowserOptionHelper.GetOptions($"Click Command"), driver =>
            {
                //Find the button in the CommandBar
                var ribbon = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container]),
            TimeSpan.FromSeconds(5));

                if (ribbon == null)
                {
                    ribbon = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid]),
                        TimeSpan.FromSeconds(5),
                        "Unable to find the ribbon.");
                }

                //Get the CommandBar buttons
                var items = ribbon.FindElements(By.TagName("button"));

                //Is the button in the ribbon?
                if (items.Any(x => x.GetAttribute("aria-label").Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    items.FirstOrDefault(x => x.GetAttribute("aria-label").Equals(name, StringComparison.OrdinalIgnoreCase)).Click(true);
                    driver.WaitForTransaction();
                }
                else
                {
                    //Is the button in More Commands?
                    var moreCommands = items.FirstOrDefault(x => x.HasAttribute("data-id") && x.GetAttribute("data-id").Equals("OverflowButton", StringComparison.OrdinalIgnoreCase));
                    if (moreCommands != null)
                    {
                        //Click More Commands
                        moreCommands.Click(true);
                        driver.WaitForTransaction();

                        //Click the button
                        if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Button].Replace("[NAME]", name))))
                        {
                            driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.CommandBar.Button].Replace("[NAME]", name)), TimeSpan.FromSeconds(5), $"Unable to click on button: {name}").Click(true);
                            driver.WaitForTransaction();
                        }
                        else
                            throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");
                    }
                    else
                        throw new InvalidOperationException($"No command with the name '{name}' exists inside of Commandbar.");
                }

                if (!string.IsNullOrEmpty(subname))
                {
                    var submenu = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.MoreCommandsMenu]));

                    var subbutton = submenu.FindElements(By.TagName("button")).FirstOrDefault(x => x.Text == subname);

                    if (subbutton != null)
                    {
                        subbutton.Click(true);
                    }
                    else
                        throw new InvalidOperationException($"No sub command with the name '{subname}' exists inside of Commandbar.");

                    if (!string.IsNullOrEmpty(subSecondName))
                    {
                        var subSecondmenu = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.CommandBar.MoreCommandsMenu]));

                        var subSecondbutton = subSecondmenu.FindElements(By.TagName("button")).FirstOrDefault(x => x.Text == subSecondName);

                        if (subSecondbutton != null)
                        {
                            subSecondbutton.Click(true);
                        }
                        else
                            throw new InvalidOperationException($"No sub command with the name '{subSecondName}' exists inside of Commandbar.");
                    }
                }

                driver.WaitForTransaction();

                return true;
            });
        }

        #endregion

        #region https://github.com/DynamicHands/Crm.Specflow/issues/126

        public static void Save(this WebClient client, LocalizedTexts buttonTexts, int lcid)
        {
            client.ClickCommand(buttonTexts[Constants.LocalizedTexts.SaveButton, lcid]);

            client.HandleSaveDialog();
            client.Browser.Driver.WaitForTransaction();
        }

        private static BrowserCommandResult<bool> HandleSaveDialog(this WebClient client)
        {
            //If you click save and something happens, handle it.  Duplicate Detection/Errors/etc...
            //Check for Dialog and figure out which type it is and return the dialog type.

            //Introduce think time to avoid timing issues on save dialog
            client.Browser.ThinkTime(1000);

            return client.Execute(BrowserOptionHelper.GetOptions($"Validate Save"), driver =>
            {
                //Is it Duplicate Detection?
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionWindowMarker])))
                {
                    if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows])))
                    {
                        //Select the first record in the grid
                        driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows]))[0].Click(true);

                        //Click Ignore and Save
                        driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton])).Click(true);
                        driver.WaitForTransaction();
                    }
                }

                //Is it an Error?
                
                if (driver.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_ErrorDialog), out var errorDialog))
                {
                    var errorDetails = errorDialog.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Subtitle));

                    if (!string.IsNullOrEmpty(errorDetails.Text))
                        throw new InvalidOperationException(errorDetails.Text);
                }


                return true;
            });
        }

        #endregion


    }
}
