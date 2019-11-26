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
            if (driver.TryFindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Subtitle), out IWebElement subTitle))
                return subTitle.Text;
            else
                return null;
        }

        public static IReadOnlyCollection<FormNotification> GetFormNotifications(this WebClient client)
        {
            return client.Execute(BrowserOptionHelper.GetOptions($"Set Value"), driver =>
            {
                List<FormNotification> notifications = new List<FormNotification>();

                if(!driver.TryFindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_NotificationBar),
                    out var notificationBar))
                {
                    return notifications;
                }

                if(notificationBar.TryFindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_ExpandButton), out var expandButton))
                {
                    if(!Convert.ToBoolean(notificationBar.GetAttribute("aria-expanded")))
                        expandButton.Click();

                    notificationBar = driver.WaitUntilAvailable(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.FlyoutRoot), TimeSpan.FromSeconds(2), "Failed to open the form notifications");
                }

                var notificationList = notificationBar.FindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_NotificationList));
                var notificationListItems = notificationList.FindElements(By.TagName("li"));

                foreach(var item in notificationListItems)
                {
                    var icon = item.FindElement(Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_NotificationTypeIcon));

                    var notification = new FormNotification
                    {
                        Message = item.GetAttribute("aria-label")
                    };

                    if (icon.HasClass("MarkAsLost-symbol"))
                        notification.Type = FormNotificationType.Error;
                    else if (icon.HasClass("Warning-symbol"))
                        notification.Type = FormNotificationType.Warning;
                    else if (icon.HasClass("InformationIcon-symbol"))
                        notification.Type = FormNotificationType.Information;
                    else
                        throw new TestExecutionException(Constants.ErrorCodes.UNKNOWN_FORM_NOTIFICATION_TYPE, icon.GetAttribute("class"));

                    notifications.Add(notification);
                }



                return notifications;

            }).Value;
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

       
        

    }
}
