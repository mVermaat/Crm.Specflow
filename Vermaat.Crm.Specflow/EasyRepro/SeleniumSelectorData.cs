using OpenQA.Selenium;
using System.Collections.Generic;

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
            { SeleniumSelectorItems.Entity_AccessDialog,  "//div[@data-id='AccessCheckerDialog']"} ,
            { SeleniumSelectorItems.Entity_AccessDialogItems, ".//div[@data-id='accesscheckercontrol-accesscheckercontrol_container']//button" },
            { SeleniumSelectorItems.Entity_BusinessProcessFlow_StageElement, "//ul[@id='MscrmControls.Containers.ProcessBreadCrumb-headerStageContainer']/li" },
            { SeleniumSelectorItems.Entity_CompositeControls, "//div[starts-with(@data-control-name,'[NAME]_compositionLinkControl_')]" },
            { SeleniumSelectorItems.Entity_FormId, "//div[@id='navigationcontextprovider']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_NotificationBar, "//div[@data-id='notificationWrapper']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_ExpandButton, ".//span[@id='notificationExpandIcon']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_NotificationList, ".//ul[@data-id='notificationList']" },
            { SeleniumSelectorItems.Entity_FormNotifcation_NotificationTypeIcon, ".//span[contains(@id,'notification_icon_')]" },
            { SeleniumSelectorItems.Entity_FormNotification_NotificationMessage, ".//span[@data-id='warningNotification']" },
            { SeleniumSelectorItems.Entity_MissingPermisions_DialogRoot, "//div[@role='alertdialog']" },
            { SeleniumSelectorItems.Entity_MissingPermisions_MoreItemFlyout, "//div[contains(@class, 'ms-ContextualMenu-container')]" },
            { SeleniumSelectorItems.Entity_Ribbon_Button, ".//button[starts-with(@aria-label,'[NAME]') and span/span/text() = '[NAME]']" },
            { SeleniumSelectorItems.Entity_Ribbon_Flyout_Container, "//ul[@data-id='OverflowFlyout']" },
            { SeleniumSelectorItems.Entity_Ribbon_More_Commands, ".//button[@data-id='OverflowButton']" },
            { SeleniumSelectorItems.Entity_QuickCreate_DialogRoot, "//section[@data-id='quickCreateRoot']" },
            { SeleniumSelectorItems.Entity_QuickCreate_Notification_Window, "//div[contains(@data-id,'ToastNotification_quickcreate')]" },
            { SeleniumSelectorItems.Entity_QuickCreate_OpenChildButton, ".//p[text() = '[NAME]']" },
            { SeleniumSelectorItems.Entity_SaveStatus, "//span[@data-id='header_saveStatus']" },
            { SeleniumSelectorItems.Entity_SubGrid, "//div[@id=\"dataSetRoot_[NAME]\"]" },
            { SeleniumSelectorItems.Entity_SubGrid_ButtonList, ".//ul[@data-id='CommandBar']" },
            { SeleniumSelectorItems.Entity_SubGrid_Button, ".//button[contains(@data-id,'[NAME]')]" },
            { SeleniumSelectorItems.FlyoutRoot, "//div[@data-id='__flyoutRootNode']" },
            { SeleniumSelectorItems.Entity_ScriptErrorDialog, "//*[@id='dialogTitleText']" },
            { SeleniumSelectorItems.Entity_FormLoad, "//ul[contains(@id, 'tablist_')]" },
            { SeleniumSelectorItems.Entity_MoreTabs, ".//button[@data-id='more_button']" },
            { SeleniumSelectorItems.Entity_DateContainer, "//div[contains(@data-id,'[NAME].fieldControl._datecontrol-date-container')]" },
            { SeleniumSelectorItems.Entity_TimeContainer, "//div[contains(@data-id,'[NAME].fieldControl._timecontrol-datetime-container')]" },
            { SeleniumSelectorItems.Entity_LookupDeleteItem, ".//*[contains(@data-id, '[NAME].fieldControl-LookupResultsDropdown_[NAME]_selected_tag_delete')]" },
            { SeleniumSelectorItems.Entity_LookupItemContainer, ".//div[@data-id='[NAME].fieldControl-Lookup_[NAME]']" },
            { SeleniumSelectorItems.Entity_Footer_Status, "//span[@data-id='edit-form-status-message']" },
            { SeleniumSelectorItems.Entity_FormState_LockedIcon, "//div[@data-id='[NAME]-locked-icon']" },
            { SeleniumSelectorItems.Entity_FormState_RequiredOrRecommended, "//div[contains(@id, '[NAME]-required-icon')] | //span[contains(@data-id, '[NAME]-required-icon')]" },
            { SeleniumSelectorItems.Dialog_Container, "//div[starts-with(@id, 'dialogContentContainer_')]" },
            { SeleniumSelectorItems.Entity_BPFFieldContainer, "//div[@id='[NAME]-[NAME]-FieldSectionItemContainer']" },
            { SeleniumSelectorItems.Entity_FieldContainer, "//div[@data-id='[NAME]']" },
            { SeleniumSelectorItems.Entity_PCFControl_Container, ".//div[@data-id='[NAME].fieldControl_container']" },
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
