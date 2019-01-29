using Microsoft.Xrm.Sdk;
using System;

namespace Vermaat.Crm.Specflow.Commands
{
    class MoveToNextBusinessProcessStageCommand : ApiOnlyCommand
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
                throw new InvalidOperationException("Current stage can't be found");
            if (currentStage + 1 >= path.Length)
                throw new InvalidOperationException("Current stage be the last");

            var processRecord = BusinessProcessFlowHelper.GetProcessRecord(_crmContext, crmRecord, instance.Id);
            processRecord["activestageid"] = path[currentStage + 1].ToEntityReference();
            _crmContext.Service.Update(processRecord);
        }
    }
}
