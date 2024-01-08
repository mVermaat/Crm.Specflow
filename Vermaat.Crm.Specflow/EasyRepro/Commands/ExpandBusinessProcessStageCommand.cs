using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Xrm.Sdk.Query;
using OpenQA.Selenium;
using System;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class ExpandBusinessProcessStageCommand : ISeleniumCommand
    {
        private readonly string _stageName;

        public ExpandBusinessProcessStageCommand(string stageName)
        {
            _stageName = stageName;
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            var processStages = browserInteraction.Driver.FindElements(By.XPath(AppElements.Xpath[AppReference.BusinessProcessFlow.NextStage_UCI]));

            foreach (var processStage in processStages)
            {
                var idAttribute = processStage.GetAttribute("id");
                var stageId = Guid.Parse(idAttribute.Substring(62));
                var name = GlobalTestingContext.ConnectionManager.AdminConnection.Retrieve("processstage", stageId,
                    new ColumnSet("stagename")).GetAttributeValue<string>("stagename");

                if (name.Equals(_stageName, StringComparison.OrdinalIgnoreCase))
                {
                    processStage.Click();
                    return CommandResult.Success();
                }
            }
            return CommandResult.Fail(true, Constants.ErrorCodes.BPF_STAGE_NOT_FOUND, _stageName);
        }
    }
}
