using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    internal class SeleniumSelectorData
    {
        private Dictionary<SeleniumSelectorItems, string> selectors = new Dictionary<SeleniumSelectorItems, string>()
        {
            { SeleniumSelectorItems.Dialog_Subtitle, "//h2[@id='subtitle']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_NotificationBar, "//div[@data-id='notificationWrapper']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_ExpandButton, ".//span[@id='notificationExpandIcon']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_NotificationList, ".//ul[@data-id='notificationList']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_NotificationTypeIcon, ".//span[contains(@id,'notification_icon_')]" },
            { SeleniumSelectorItems.Entity_SubGrid, "//div[@id=\"dataSetRoot_[NAME]\"]" },
            { SeleniumSelectorItems.Entity_SubGrid_ButtonList, ".//ul[@data-id='CommandBar']" },
            { SeleniumSelectorItems.Entity_SubGrid_Button, ".//button[contains(@data-id,'[NAME]')]" },
            { SeleniumSelectorItems.FlyoutRoot, "//div[@id='__flyoutRootNode']" },
            { SeleniumSelectorItems.Entity_ScriptErrorDialog, "//*[@id='dialogTitleText']" },
            { SeleniumSelectorItems.Entity_FormLoad, "id(\"tablist\")" },
            { SeleniumSelectorItems.Entity_MoreTabs, ".//button[@data-id='more_button']" },
            { SeleniumSelectorItems.Entity_DateContainer, "//div[contains(@data-id,'[NAME].fieldControl._datecontrol-date-container')]" },
            { SeleniumSelectorItems.Entity_Footer_Status, "//span[@data-id='edit-form-status-message']" },
        };

        public By GetXPathSeleniumSelector(SeleniumSelectorItems itemName)
        {
            return By.XPath(selectors[itemName]);
        }

        public By GetXPathSeleniumSelector(SeleniumSelectorItems itemName, string nameReplacement)
        {
            return By.XPath(selectors[itemName].Replace("[NAME]", nameReplacement));
        }
    }
}
