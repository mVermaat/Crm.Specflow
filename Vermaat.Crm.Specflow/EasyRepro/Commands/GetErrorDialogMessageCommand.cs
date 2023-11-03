using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class GetErrorDialogMessageCommand : ISeleniumCommandFunc<string>
    {
        private readonly bool _dialogMandatory;

        public GetErrorDialogMessageCommand(bool dialogMandatory)
        {
            _dialogMandatory = dialogMandatory;
        }

        public CommandResult<string> Execute(BrowserInteraction browserInteraction)
        {
            var errorDialog = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_ErrorDialog), TimeSpan.FromSeconds(5));
            if (errorDialog != null)
            {
                var errorDetails = errorDialog.FindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Subtitle));
                return CommandResult<string>.Success(errorDetails.Text);
            }
            else if (_dialogMandatory)
                return CommandResult<string>.Fail(true, Constants.ErrorCodes.ERROR_DIALOG_NOT_FOUND);
            else
                return CommandResult<string>.Success(null);
        }
    }
}
