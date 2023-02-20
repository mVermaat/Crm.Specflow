using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
                var divs = processStage.FindElements(By.TagName("div"));

                //Click the Label of the Process Stage if found
                foreach (var div in divs)
                {
                    if (div.Text.Equals(_stageName, StringComparison.OrdinalIgnoreCase))
                    {
                        div.Click();
                        return CommandResult.Success();
                    }
                }
            }
            return CommandResult.Fail(true, Constants.ErrorCodes.BPF_STAGE_NOT_FOUND, _stageName);
        }
    }
}
