using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class SeleniumSelectorData
    {
        private readonly Dictionary<SeleniumSelectorItems, string> _selectors = new Dictionary<SeleniumSelectorItems, string>()
        {
            { SeleniumSelectorItems.Dialog_Subtitle, ".//h2[@data-id='errorDialog_subtitle']" },
            { SeleniumSelectorItems.Dialog_ErrorDialog, "//div[@data-id='errorDialogdialog']" },
            { SeleniumSelectorItems.DuplicateDetection_Grid, "//div[@data-id='manage_duplicates_grid.fieldControl.ManageDuplicatesGrid_container']"},
            { SeleniumSelectorItems.DuplicateDetection_SelectedItems, ".//div[@aria-checked='true']/.."},
            { SeleniumSelectorItems.Entity_FormNotifcation_NotificationBar, "//div[@data-id='notificationWrapper']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_ExpandButton, ".//span[@id='notificationExpandIcon']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_NotificationList, ".//ul[@data-id='notificationList']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_NotificationTypeIcon, ".//span[contains(@id,'notification_icon_')]" },
            { SeleniumSelectorItems.Entity_FormNotification_NotificationMessage, ".//span[@data-id='warningNotification']" },
            { SeleniumSelectorItems.Entity_Ribbon_Button, ".//button[starts-with(@aria-label,'[NAME]') and span/span/text() = '[NAME]']" },
            { SeleniumSelectorItems.Entity_Ribbon_Flyout_Container, "//ul[@data-id='OverflowFlyout']" },
            { SeleniumSelectorItems.Entity_Ribbon_More_Commands, ".//button[@data-id='OverflowButton']" },
            { SeleniumSelectorItems.Entity_QuickCreate_Notification_Window, "//div[contains(@data-id,'ToastNotification_quickcreate')]" },
            { SeleniumSelectorItems.Entity_QuickCreate_OpenChildButton, ".//p[text() = '[NAME]']" },
            { SeleniumSelectorItems.Entity_SaveStatus, "//span[@data-id='header_saveStatus']" },
            { SeleniumSelectorItems.Entity_SubGrid, "//div[@id=\"dataSetRoot_[NAME]\"]" },
            { SeleniumSelectorItems.Entity_SubGrid_ButtonList, ".//ul[@data-id='CommandBar']" },
            { SeleniumSelectorItems.Entity_SubGrid_Button, ".//button[contains(@data-id,'[NAME]')]" },
            { SeleniumSelectorItems.FlyoutRoot, "//div[@id='__flyoutRootNode']" },
            { SeleniumSelectorItems.Entity_ScriptErrorDialog, "//*[@id='dialogTitleText']" },
            { SeleniumSelectorItems.Entity_FormLoad, "//ul[contains(@id, 'tablist_')]" },
            { SeleniumSelectorItems.Entity_MoreTabs, ".//button[@data-id='more_button']" },
            { SeleniumSelectorItems.Entity_DateContainer, "//div[contains(@data-id,'[NAME].fieldControl._datecontrol-date-container')]" },
            { SeleniumSelectorItems.Entity_DateTime_Time_Input, ".//input[contains(@id, 'DatePicker')]" },
            { SeleniumSelectorItems.Entity_Footer_Status, "//span[@data-id='edit-form-status-message']" },
            { SeleniumSelectorItems.Entity_FormState_LockedIcon, "//div[@data-id='[NAME]-locked-icon']" },
            { SeleniumSelectorItems.Entity_FormState_RequiredOrRecommended, "//div[contains(@id, '[NAME]-required-icon')]" },
            { SeleniumSelectorItems.Dialog_Container, "//div[starts-with(@id, 'dialogContentContainer_')]" },
            { SeleniumSelectorItems.Entity_FieldContainer, "//div[@data-id='[NAME]']" },
            { SeleniumSelectorItems.Dialog_OK, "//button[@data-id='ok_id']" },
            { SeleniumSelectorItems.Entity_Header, "//button[@data-id='header_overflowButton']" },
            { SeleniumSelectorItems.Popup_TeachingBubble_CloseButton, "//button[contains(@class, 'ms-TeachingBubble-closebutton')]/span" }
        };

        public By GetXPathSeleniumSelector(SeleniumSelectorItems itemName)
        {
            if (_selectors.TryGetValue(itemName, out var selector))
                return By.XPath(selector);
            else
                throw new TestExecutionException(Constants.ErrorCodes.SELECTOR_NOT_FOUND, itemName);
        }

        public By GetXPathSeleniumSelector(SeleniumSelectorItems itemName, string nameReplacement)
        {
            if (_selectors.TryGetValue(itemName, out var selector))
                return By.XPath(selector.Replace("[NAME]", nameReplacement));
            else
                throw new TestExecutionException(Constants.ErrorCodes.SELECTOR_NOT_FOUND, itemName);
        }
    }
}
