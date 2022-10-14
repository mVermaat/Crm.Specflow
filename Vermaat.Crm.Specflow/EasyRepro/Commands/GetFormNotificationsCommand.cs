using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class GetFormNotificationsCommand : ISeleniumCommandFunc<IReadOnlyCollection<FormNotification>>
    {
        public CommandResult<IReadOnlyCollection<FormNotification>> Execute(BrowserInteraction browserInteraction)
        {
            List<FormNotification> notifications = new List<FormNotification>();

            if (!browserInteraction.Driver.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_NotificationBar),
                out var notificationBar))
            {
                return CommandResult<IReadOnlyCollection<FormNotification>>.Success(notifications);
            }

            if (notificationBar.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_ExpandButton), out var expandButton))
            {
                if (!Convert.ToBoolean(notificationBar.GetAttribute("aria-expanded")))
                    expandButton.Click();

                notificationBar = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.FlyoutRoot), TimeSpan.FromSeconds(2), "Failed to open the form notifications");
            }

            var notificationList = notificationBar.FindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_NotificationList));
            var notificationListItems = notificationList.FindElements(By.TagName("li"));

            foreach (var item in notificationListItems)
            {
                var icon = item.FindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotifcation_NotificationTypeIcon));
                var message = item.FindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormNotification_NotificationMessage));


                var notification = new FormNotification
                {
                    Message = message.GetAttribute("innerText")
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
            return CommandResult<IReadOnlyCollection<FormNotification>>.Success(notifications);
        }
    }
}
