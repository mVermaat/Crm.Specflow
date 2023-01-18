using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class SetCustomControlValueCommand : ISeleniumCommand
    {
        private readonly string _attributeName;
        private readonly string _controlName;

        public SetCustomControlValueCommand(string attributeName, string controlName)
        {
            _attributeName = attributeName;
            _controlName = controlName;
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            var formContext = browserInteraction.Driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.FormContext]));

            if (formContext == null)
                return CommandResult.Fail(true, Constants.ErrorCodes.ELEMENT_NOT_FOUND, $"FormContext for {_attributeName}");

            var fieldContainer = formContext.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", _attributeName)));
            if (fieldContainer == null)
                return CommandResult.Fail(true, Constants.ErrorCodes.ELEMENT_NOT_FOUND, $"FieldContainer for {_attributeName}");

            var pcfContainer = fieldContainer.FindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_PCFControl_Container, _attributeName));

            return SetValue(browserInteraction, pcfContainer, _attributeName, _controlName);

        }

        protected virtual CommandResult SetValue(BrowserInteraction browserInteraction, IWebElement pcfContainer, string attributeName, string controlName)
        {
            return CommandResult.Fail(false, Constants.ErrorCodes.PCF_CONTROL_NOT_IMPLEMENTED, controlName, attributeName);
        }
    }
}
