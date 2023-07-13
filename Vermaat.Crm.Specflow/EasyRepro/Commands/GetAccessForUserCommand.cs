using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var accessItems = dialogRoot.FindElements(browserInteraction.Selectors
                .GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_AccessDialogItems));

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

            return ParseAccessData(unstructuredAccessData);


        }

        private CommandResult<UserAccessData> ParseAccessData(Dictionary<string, bool> unstructuredAccessData)
        {
            var result = new UserAccessData();

            try
            {
                result.HasAppendAccess = unstructuredAccessData["Append"];
                result.HasAppendToAccess = unstructuredAccessData["Append to"];
                result.HasAssignAccess = unstructuredAccessData["Assign"];
                result.HasCreateAccess = unstructuredAccessData["Create"];
                result.HasDeleteAccess = unstructuredAccessData["Delete"];
                result.HasReadAccess = unstructuredAccessData["Read"];
                result.HasShareAccess = unstructuredAccessData["Share"];
                result.HasWriteAccess = unstructuredAccessData["Write"];

                return CommandResult<UserAccessData>.Success(result);
            }
            catch (KeyNotFoundException)
            {
                return CommandResult<UserAccessData>.Fail(false, Constants.ErrorCodes.CHECK_ACCESS_DIALOG_MISSING_PERMISSIONS, string.Join(", ", unstructuredAccessData.Keys));
            }
        }
    }
}
