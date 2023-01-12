using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class SetBusinessProcessFlowTextFieldValueCommand : ISeleniumCommand
    {
        private readonly string _logicalName;
        private readonly string _value;

        public SetBusinessProcessFlowTextFieldValueCommand(string logicalName, string value)
        {
            _logicalName = logicalName;
            _value = value;
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            var fieldContainer = browserInteraction.Driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.TextFieldContainer].Replace("[NAME]", _logicalName)));

            if (fieldContainer.TryFindElement(By.TagName("input"), out var input))
                SetValue(input);
            else if (fieldContainer.TryFindElement(By.TagName("textarea"), out var textarea))
                SetValue(textarea);
            else
               return CommandResult.Fail(false, Constants.ErrorCodes.FIELD_NOT_ON_FORM, _logicalName);

            return CommandResult.Success();
        }

        private void SetValue(IWebElement input)
        {
            input.Click(true);

            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Backspace);

            if (!string.IsNullOrEmpty(_value))
                input.SendKeys(_value);

            input.SendKeys(Keys.Tab);
        }
    }
}
