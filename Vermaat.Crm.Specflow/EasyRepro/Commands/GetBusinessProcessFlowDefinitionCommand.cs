using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class GetBusinessProcessFlowDefinitionCommand : ISeleniumCommandFunc<BusinessProcessFlowDefinition>
    {
        public CommandResult<BusinessProcessFlowDefinition> Execute(BrowserInteraction browserInteraction)
        {
            if (browserInteraction.Driver.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(
                SeleniumSelectorItems.Entity_BusinessProcessFlow_StageElement), out var bpfStageElement))
            {
                var id = bpfStageElement.GetAttribute("id");

                if (string.IsNullOrEmpty(id) || !id.StartsWith("MscrmControls.Containers.ProcessBreadCrumb-processHeaderStage_") ||
                    !Guid.TryParse(id.Substring(62), out var stageId))
                {
                    return CommandResult<BusinessProcessFlowDefinition>.Fail(false, Constants.ErrorCodes.BUSINESS_PROCESS_FLOW_STAGE_ID_CANT_BE_FOUND, id);
                }

                var stage = GlobalTestingContext.ConnectionManager.AdminConnection.Retrieve("processstage", stageId, new ColumnSet("processid"));
                var process = GlobalTestingContext.ConnectionManager.CurrentConnection.Retrieve(stage.GetAttributeValue<EntityReference>("processid"), new ColumnSet("uidata"));

                return CommandResult<BusinessProcessFlowDefinition>.Success(JsonConvert.DeserializeObject<BusinessProcessFlowDefinition>(
                    process.GetAttributeValue<string>("uidata")));
            }
            else
                return CommandResult<BusinessProcessFlowDefinition>.Success(null);
        }
    }
}
