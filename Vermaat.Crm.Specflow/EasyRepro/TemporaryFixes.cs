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
        /// <param name="date">DateTime value.</param>
        /// <param name="format">Datetime format matching Short Date & Time formatting personal options.</param>
        /// <example>xrmApp.Entity.SetValue("birthdate", DateTime.Parse("11/1/1980"));</example>
        public static BrowserCommandResult<bool> SetValueFix(this WebClient client, string field, DateTime date, string format = "M/d/yyyy h:mm tt")
        {
            return client.Execute(BrowserOptionHelper.GetOptions($"Set Value"), driver =>
            {
                driver.WaitForTransaction();

                var dateContainer = SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_DateContainer, field);
                var dateField = AppElements.Xpath[AppReference.Entity.FieldControlDateTimeInputUCI].Replace("[FIELD]", field);

                if (driver.HasElement(dateContainer))
                {
                    var container = driver.WaitUntilAvailable(dateContainer);
                    var fieldElement = container.FindElement(By.XPath("." + dateField));

                    fieldElement.SendKeys(Keys.Control + "a");
                    fieldElement.SendKeys(Keys.Backspace);

                    if (driver.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_DateTime_Time_Input, field), out var timeElement))
                    {
                        driver.ClearFocus();
                        driver.WaitForTransaction();
                        try
                        {
                            driver.WaitFor(d => string.IsNullOrWhiteSpace(timeElement.GetAttribute("value")) || timeElement.GetAttribute("value") == "---");
                        }
                        catch (WebDriverTimeoutException ex)
                        {
                            throw new InvalidOperationException($"Timeout after 30 seconds. Expected cleared DateTime. Actual: {fieldElement.GetAttribute("value")}", ex);
                        }
                    }

                    fieldElement.SendKeys(date.ToString(format));

                    try
                    {
                        driver.WaitFor(d => fieldElement.GetAttribute("value") == date.ToString(format));
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        throw new InvalidOperationException($"Timeout after 30 seconds. Expected: {date.ToString(format)}. Actual: {fieldElement.GetAttribute("value")}", ex);
                    }
                    driver.ClearFocus();
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
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
