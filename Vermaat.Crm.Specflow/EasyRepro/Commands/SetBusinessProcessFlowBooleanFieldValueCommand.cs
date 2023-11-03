using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class SetBusinessProcessFlowBooleanFieldValueCommand : ISeleniumCommand
    {
        private readonly BooleanItem _value;
        private readonly string _controlName;

        public SetBusinessProcessFlowBooleanFieldValueCommand(BooleanItem value)
        {
            _value = value;
            _controlName = $"{Constants.CRM.BUSINESS_PROCESS_FLOW_CONTROL_PREFIX}{value.Name}";
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            var root = browserInteraction.Driver.FindElement(
                browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_BPFFieldContainer, _controlName));

            var select = new SelectElement(root.FindElement(By.TagName("select")));
            var isSelected = select.SelectedOption.GetAttribute("value").Equals("1");

            if (isSelected != _value.Value)
            {
                select.SelectByValue(_value.Value ? "1" : "0");
            }

            return CommandResult.Success();

        }
    }
}
