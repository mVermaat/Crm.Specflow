using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class CheckFieldRequiredStateCommand : ISeleniumCommandFunc<RequiredState>
    {
        private readonly string _logicalName;

        public CheckFieldRequiredStateCommand(string logicalName)
        {
            _logicalName = logicalName;
        }

        public CommandResult<RequiredState> Execute(BrowserInteraction browserInteraction)
        {
            IWebElement fieldContainer = browserInteraction.Driver.WaitUntilAvailable(
                By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", _logicalName)), TimeSpan.FromSeconds(2));

            if (fieldContainer == null)
                throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, _logicalName);

            if (fieldContainer.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormState_RequiredOrRecommended, _logicalName), out IWebElement requiredElement))
            {
                if (requiredElement.GetAttribute("innerText") == "*")
                    return CommandResult<RequiredState>.Success(RequiredState.Required);
                else
                    return CommandResult<RequiredState>.Success(RequiredState.Recommended);
            }
            else
            {
                return CommandResult<RequiredState>.Success(RequiredState.Optional);
            }
        }
    }
}
