using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.FormLoadConditions;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class OpenQuickCreatedRecordCommand : ISeleniumCommand
    {
        private readonly string _childEntityName;

        public OpenQuickCreatedRecordCommand(string childEntityName)
        {
            _childEntityName = childEntityName;
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {

            var window = browserInteraction.Driver.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_QuickCreate_Notification_Window));
            window.WaitUntilClickable(
                SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_QuickCreate_OpenChildButton, browserInteraction.LocalizedTexts[Constants.LocalizedTexts.QuickCreateViewRecord, browserInteraction.UiLanguageCode]),
                TimeSpan.FromSeconds(5),
                null,
                () => throw new TestExecutionException(Constants.ErrorCodes.QUICK_CREATE_CHILD_NOT_AVAILABLE)).Click();

            HelperMethods.WaitForFormLoad(browserInteraction.Driver, new FormIsOfEntity(_childEntityName));

            return CommandResult.Success();
        }
    }
}
