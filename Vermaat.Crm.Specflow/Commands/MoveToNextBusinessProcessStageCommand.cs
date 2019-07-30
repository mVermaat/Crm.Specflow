using Microsoft.Xrm.Sdk;
using System;

namespace Vermaat.Crm.Specflow.Commands
{
    public class MoveToNextBusinessProcessStageCommand : ApiOnlyCommand
    {
        private readonly string _alias;

        public MoveToNextBusinessProcessStageCommand(CrmTestingContext crmContext, string alias) : base(crmContext)
        {
            _alias = alias;
        }

        public override void Execute()
        {
            EntityReference crmRecord = _crmContext.RecordCache[_alias];
            var instance = BusinessProcessFlowHelper.GetProcessInstanceOfRecord(_crmContext, crmRecord);
            var path = BusinessProcessFlowHelper.GetActivePath(_crmContext, instance);

            int currentStage = -1;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i].Id == instance.GetAttributeValue<Guid>("processstageid"))
                {
                    currentStage = i;
                }
            }

            if (currentStage == -1)
                throw new TestExecutionException(Constants.ErrorCodes.CURRENT_BUSINESS_PROCESS_STAGE_NOT_FOUND);
            if (currentStage + 1 >= path.Length)
                throw new TestExecutionException(Constants.ErrorCodes.BUSINESS_PROCESS_STAGE_CANNOT_BE_LAST);

            var processRecord = BusinessProcessFlowHelper.GetProcessRecord(_crmContext, crmRecord, instance.Id);
            processRecord["activestageid"] = path[currentStage + 1].ToEntityReference();
            GlobalTestingContext.ConnectionManager.CurrentConnection.Update(processRecord);
        }
    }
}
