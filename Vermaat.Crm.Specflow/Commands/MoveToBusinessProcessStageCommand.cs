using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Commands
{
    public class MoveToBusinessProcessStageCommand : ApiOnlyCommand
    {
        private readonly string _alias;
        private readonly string _stageName;

        public MoveToBusinessProcessStageCommand(CrmTestingContext crmContext, string alias, string stageName) : base(crmContext)
        {
            this._alias = alias;
            this._stageName = stageName;
        }

        public override void Execute()
        {
            EntityReference crmRecord = _crmContext.RecordCache[_alias];
            var instance = BusinessProcessFlowHelper.GetProcessInstanceOfRecord(_crmContext, crmRecord);
            var path = BusinessProcessFlowHelper.GetActivePath(_crmContext, instance);

            int currentStage = -1;
            int desiredStage = -1;

            for (int i = 0; i < path.Length && (currentStage == -1 || desiredStage == -1); i++)
            {
                if (_stageName == path[i].GetAttributeValue<string>("stagename"))
                    desiredStage = i;
                if (path[i].Id == instance.GetAttributeValue<Guid>("processstageid"))
                    currentStage = i;
            }

            if (currentStage == -1)
                throw new TestExecutionException(Constants.ErrorCodes.CURRENT_BUSINESS_PROCESS_STAGE_NOT_FOUND);
            if (desiredStage == -1)
                throw new TestExecutionException(Constants.ErrorCodes.BUSINESS_PROCESS_STAGE_NOT_IN_ACTIVE_PATH, _stageName);

            if (currentStage == desiredStage)
                return;

            var processRecord = BusinessProcessFlowHelper.GetProcessRecord(_crmContext, crmRecord, instance.Id);

            if (desiredStage < currentStage)
            {
                processRecord["activestageid"] = path[desiredStage].ToEntityReference();
                GlobalTestingContext.ConnectionManager.CurrentConnection.Update(processRecord);
                return;
            }

            while (desiredStage > currentStage)
            {
                processRecord["activestageid"] = path[currentStage + 1].ToEntityReference();
                GlobalTestingContext.ConnectionManager.CurrentConnection.Update(processRecord);
                currentStage++;
            }
        }
    }
}
