using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public static class TemporaryFixes
    {
        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.Entity.SetValue("firstname", "Test");</example>
        public static BrowserCommandResult<bool> SetValueFix(this WebClient client, string field, string value)
        {
            return client.Execute(BrowserOptionHelper.GetOptions($"Set Value"), driver =>
            {
                var query = By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field));
                IWebElement fieldContainer = WaitUntilClickable(driver, query, TimeSpan.FromSeconds(5), null, null);

                if (fieldContainer == null)
                {
                    throw new TestExecutionException(Constants.ErrorCodes.ELEMENT_NOT_INTERACTABLE, $"Field {field} is probably locked or invisible");
                }

                IWebElement input;
                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    input = fieldContainer.FindElement(By.TagName("input"));

                }
                else if (fieldContainer.FindElements(By.TagName("textarea")).Count > 0)
                {
                    input = fieldContainer.FindElement(By.TagName("textarea"));
                }
                else
                {
                    throw new Exception($"Field with name {field} does not exist.");
                }

                if (input != null)
                {
                    input.SendKeys(Keys.Control + "a");
                    input.SendKeys(Keys.Backspace);

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        input.SendKeys(value);
                    }

                    input.SendKeys(Keys.Tab + Keys.Tab);
                }

                return true;
            });
        }

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

        /// <summary>
        /// Generic method to help click on any item which is clickable or uniquely discoverable with a By object.
        /// </summary>
        /// <param name="by">The xpath of the HTML item as a By object</param>
        /// <returns>True on success, Exception on failure to invoke any action</returns>
        public static BrowserCommandResult<bool> SelectTabFix(this WebClient client, string tabName, string subTabName = "", int thinkTime = Microsoft.Dynamics365.UIAutomation.Browser.Constants.DefaultThinkTime)
        {
            client.Browser.ThinkTime(thinkTime);

            return client.Execute(BrowserOptionHelper.GetOptions($"Select Tab"), driver =>
            {
                IWebElement tabList = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TabList]));

                ClickTab(driver, tabList, ".//li[@title='{0}']", tabName);

                //Click Sub Tab if provided
                if (!string.IsNullOrEmpty(subTabName))
                {
                    ClickTab(driver, tabList, AppElements.Xpath[AppReference.Entity.SubTab], subTabName);
                }

                driver.WaitForTransaction();
                return true;
            });
        }

        private static void ClickTab(IWebDriver driver, IWebElement tabList, string xpath, string name)
        {
            // Look for the tab in the tab list, else in the more tabs menu
            IWebElement searchScope = null;
            if (tabList.HasElement(By.XPath(string.Format(xpath, name))))
            {
                searchScope = tabList;

            }
            else if (tabList.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_MoreTabs), out IWebElement moreTabsButton))
            {
                moreTabsButton.Click();
                searchScope = driver.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.FlyoutRoot));
            }


            if (searchScope != null && searchScope.TryFindElement(By.XPath(string.Format(xpath, name)), out IWebElement listItem))
            {
                listItem.Click(true);
            }
            else
            {
                throw new Exception($"The tab with name: {name} does not exist");
            }

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
    }
}
