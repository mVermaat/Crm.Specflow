using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class SetBusinessProcessFlowDateTimeFieldValueCommand : SetDateTimeFieldValueCommand
    {
        public SetBusinessProcessFlowDateTimeFieldValueCommand(string logicalName, DateTime? value, bool dateOnly, string formatDate, string formatTime)
            : base(logicalName, value, dateOnly, formatDate, formatTime)
        {
        }

        protected override IWebElement GetDateContainer(BrowserInteraction browserInteraction, string logicalName)
        {
            return browserInteraction.Driver.WaitUntilAvailable(
                            SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_BPFFieldContainer, $"{Constants.CRM.BUSINESS_PROCESS_FLOW_CONTROL_PREFIX}{logicalName}"),
                            $"Field: {logicalName} does not exist");
        }

    }
}
