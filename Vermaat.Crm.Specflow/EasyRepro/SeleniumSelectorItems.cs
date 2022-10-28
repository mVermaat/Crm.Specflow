using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public enum SeleniumSelectorItems
    {
        Dialog_Container,
        Dialog_OK,
        Dialog_Subtitle,
        Dialog_ErrorDialog,

        DuplicateDetection_Grid,
        DuplicateDetection_SelectedItems,

        Entity_ScriptErrorDialog,

        Entity_DateContainer,
        Entity_DateTime_Time_Input,

        Entity_Footer_Status,

        Entity_FormLoad,

        Entity_FormNotifcation_NotificationBar,
        Entity_FormNotifcation_ExpandButton,
        Entity_FormNotifcation_NotificationList,
        Entity_FormNotifcation_NotificationTypeIcon,
        Entity_FormNotification_NotificationMessage,

        Entity_FormState_LockedIcon,
        Entity_FormState_RequiredOrRecommended,

        Entity_QuickCreate_Notification_Window,
        Entity_QuickCreate_OpenChildButton,

        Entity_Ribbon_Button,
        Entity_Ribbon_More_Commands,
        Entity_Ribbon_Flyout_Container,

        Entity_SaveStatus,

        Entity_SubGrid,
        Entity_SubGrid_Button,
        Entity_SubGrid_ButtonList,

        Entity_Header,

        Entity_MoreTabs,

        Entity_FieldContainer,

        FlyoutRoot,

        Popup_TeachingBubble_CloseButton
    }
}