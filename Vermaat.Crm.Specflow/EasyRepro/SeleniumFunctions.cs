using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    internal static class SeleniumFunctions
    {
        public static SeleniumSelectorData Selectors { get; } = new SeleniumSelectorData();

        public static void ClickSubgridButton(this WebClient client, string subgridName, string subgridButtonId)
        {
            client.Execute(BrowserOptionHelper.GetOptions($"Set Value"), driver =>
            {
                var subGrid = driver.WaitUntilAvailable(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SubGrid, subgridName), $"Unable to find subgrid: {subgridName}");
                
                var menuBar = subGrid.FindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SubGrid_ButtonList));

                var buttons = menuBar.FindElements(By.TagName("button"));

                var button = buttons.FirstOrDefault(b => b.GetAttribute("data-id").Contains(subgridButtonId));
                if(button != null)
                {
                    button.Click();
                    return true;
                }

                var moreCommands = buttons.FirstOrDefault(b => b.GetAttribute("data-id").Equals("OverflowButton"));
                if (moreCommands == null)
                    throw new TestExecutionException(Constants.ErrorCodes.MORE_COMMANDS_NOT_FOUND);
                moreCommands.Click();

                var flyout = driver.FindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.FlyoutRoot));
                flyout.FindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SubGrid_Button, subgridButtonId)).Click();

                return true;
            });
        }

        public static string GetErrorDialogMessage(this WebClient client)
        {
            return client.Execute(BrowserOptionHelper.GetOptions($"Get error dialog message"), driver =>
            {
                return GetErrorDialogMessage(driver);
            }).Value;
        }

        public static string GetErrorDialogMessage(IWebDriver driver)
        {
            if (driver.TryFindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_ErrorDialog), out var errorDialog))
            {
                var errorDetails = errorDialog.FindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Subtitle));
                return errorDetails.Text;
            }
            else
                return null;
        }

        public static bool TryFindElement(this IWebDriver driver, By by, out IWebElement element)
            {
                try
                {
                    element = driver.FindElement(by);
                    return true;
                }
                catch (NoSuchElementException)
                {
                    element = null;
                    return false;
                }
            }

        public static bool HasClass(this IWebElement element, string className)
        {
            return element.GetAttribute("class").Split(' ').Any(c => string.Equals(className, c, StringComparison.CurrentCultureIgnoreCase));
        }

        public static bool ScriptErrorExists(this WebClient client)
        {
            return client.Execute(BrowserOptionHelper.GetOptions($"Confirm or Cancel Confirmation Dialog"), driver =>
            {
                return driver.HasElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_ScriptErrorDialog));
            });
        }

        public static void ConfirmDuplicate(this WebClient client, bool saveIfDuplicate)
        {
            client.Execute(BrowserOptionHelper.GetOptions($"Confirm or Cancel Confirmation Dialog"), driver =>
            {
                var element = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton]), new TimeSpan(0, 0, 5));

                if (element != null)
                {
                    if (saveIfDuplicate)
                    {
                        var duplicateDetectionGrid = driver.FindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.DuplicateDetection_Grid));
                        var selectedItems = duplicateDetectionGrid.FindElements(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.DuplicateDetection_SelectedItems));

                        foreach(var item in selectedItems)
                        {
                            item.Click();
                        }

                        driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton]));
                    }
                    else
                    {
                        throw new TestExecutionException(Constants.ErrorCodes.DUPLICATE_RECORD_DETECTED);
                    }
                }

                return true;
            });

           
        }



    }
}
