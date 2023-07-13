using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class GetAccessForUserCommand : ISeleniumCommandFunc<UserAccessData>
    {
        public GetAccessForUserCommand()
        {
        }

        public CommandResult<UserAccessData> Execute(BrowserInteraction browserInteraction)
        {
            var dialogRoot = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors
                .GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_AccessDialog));
            if (dialogRoot == null)
                return CommandResult<UserAccessData>.Fail(true, Constants.ErrorCodes.CHECK_ACCESS_DIALOG_NOT_FOUND);

            var accessItems = GetAccessItems(browserInteraction, dialogRoot);

            var unstructuredAccessData = new Dictionary<string, bool>();
            foreach (var accessItem in accessItems)
            {
                var name = accessItem.GetAttribute("name");
                var iconElement = accessItem.FindElement(By.TagName("i"));

                var iconName = iconElement.GetAttribute("data-icon-name");
                if (string.IsNullOrEmpty(iconName))
                    return CommandResult<UserAccessData>.Fail(false, Constants.ErrorCodes.CHECK_ACCESS_DIALOG_UNKNOWN_ICON, "NULL");
                else if (iconName.Equals("CompletedSolid", StringComparison.InvariantCultureIgnoreCase))
                    unstructuredAccessData.Add(name, true);
                else if (iconName.Equals("Blocked", StringComparison.InvariantCultureIgnoreCase))
                    unstructuredAccessData.Add(name, false);
                else if (iconName.Equals("More"))
                    continue; // todo: more flyout
                else
                    return CommandResult<UserAccessData>.Fail(false, Constants.ErrorCodes.CHECK_ACCESS_DIALOG_UNKNOWN_ICON, iconName);
            }

             return ParseAccessData(browserInteraction, unstructuredAccessData);


        }

        private IList<IWebElement> GetAccessItems(BrowserInteraction browserInteraction, IWebElement dialogRoot)
        {
            var timeout = DateTime.Now.AddSeconds(10);

            while(DateTime.Now < timeout)
            {
                var accessItems = dialogRoot.FindElements(browserInteraction.Selectors
                .GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_AccessDialogItems));
                if (accessItems.Count > 0)
                    return accessItems;
                else
                    Thread.Sleep(100);
            }

           return new List<IWebElement>();
        }

        private CommandResult<UserAccessData> ParseAccessData(BrowserInteraction browserInteraction, Dictionary<string, bool> unstructuredAccessData)
        {
            var result = new UserAccessData();

            try
            {
                result.HasAppendAccess = unstructuredAccessData[browserInteraction.LocalizedTexts[Constants.LocalizedTexts.CheckAccessItemRead, browserInteraction.UiLanguageCode]];
                result.HasAppendToAccess = unstructuredAccessData[browserInteraction.LocalizedTexts[Constants.LocalizedTexts.CheckAccessItemAppend, browserInteraction.UiLanguageCode]];
                result.HasAssignAccess = unstructuredAccessData[browserInteraction.LocalizedTexts[Constants.LocalizedTexts.CheckAccessItemAssign, browserInteraction.UiLanguageCode]];
                result.HasCreateAccess = unstructuredAccessData[browserInteraction.LocalizedTexts[Constants.LocalizedTexts.CheckAccessItemCreate, browserInteraction.UiLanguageCode]];
                result.HasDeleteAccess = unstructuredAccessData[browserInteraction.LocalizedTexts[Constants.LocalizedTexts.CheckAccessItemDelete, browserInteraction.UiLanguageCode]];
                result.HasReadAccess = unstructuredAccessData[browserInteraction.LocalizedTexts[Constants.LocalizedTexts.CheckAccessItemRead, browserInteraction.UiLanguageCode]];
                result.HasShareAccess = unstructuredAccessData[browserInteraction.LocalizedTexts[Constants.LocalizedTexts.CheckAccessItemShare, browserInteraction.UiLanguageCode]];
                result.HasWriteAccess = unstructuredAccessData[browserInteraction.LocalizedTexts[Constants.LocalizedTexts.CheckAccessItemUpdate, browserInteraction.UiLanguageCode]];

                return CommandResult<UserAccessData>.Success(result);
            }
            catch (KeyNotFoundException)
            {
                return CommandResult<UserAccessData>.Fail(false, Constants.ErrorCodes.CHECK_ACCESS_DIALOG_MISSING_PERMISSIONS, string.Join(", ", unstructuredAccessData.Keys));
            }
        }
    }
}
