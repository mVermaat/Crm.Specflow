using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class CheckFieldLockedStateCommand : ISeleniumCommandFunc<bool>
    {
        private readonly string _controlName;

        public CheckFieldLockedStateCommand(string controlName)
        {
            _controlName = controlName;
        }

        public CommandResult<bool> Execute(BrowserInteraction browserInteraction)
        {
            var fieldContainer = browserInteraction.Driver.WaitUntilAvailable(By.XPath(
                AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", _controlName)), TimeSpan.FromSeconds(2));

            return fieldContainer != null
                ? CommandResult<bool>.Success(fieldContainer.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormState_LockedIcon, _controlName), out _))
                : throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, _controlName);
        }
    }
}
