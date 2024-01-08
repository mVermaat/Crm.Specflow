using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class CheckFieldVisibilityCommand : ISeleniumCommandFunc<bool>
    {
        private readonly SeleniumSelectorItems _selector;
        private readonly string _controlName;

        public CheckFieldVisibilityCommand(string controlName)
        {
            _selector = controlName.StartsWith(Constants.CRM.BUSINESS_PROCESS_FLOW_CONTROL_PREFIX) ?
                SeleniumSelectorItems.Entity_BPFFieldContainer : SeleniumSelectorItems.Entity_FieldContainer;
            _controlName = controlName;
        }

        public CommandResult<bool> Execute(BrowserInteraction browserInteraction)
        {
            var element = browserInteraction.Driver.WaitUntilVisible(
                SeleniumFunctions.Selectors.GetXPathSeleniumSelector(_selector, _controlName),
                TimeSpan.FromSeconds(2));

            return CommandResult<bool>.Success(element != null);
        }
    }
}
