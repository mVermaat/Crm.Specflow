using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace Vermaat.Crm.Specflow.Commands
{
    public class AssertBusinessProcessStageCommand : ApiOnlyCommand
    {
        private readonly string _alias;
        private readonly string _expectedStage;

        public AssertBusinessProcessStageCommand(CrmTestingContext crmContext, string alias, string expectedStage) : base(crmContext)
        {
            _alias = alias;
            _expectedStage = expectedStage;
        }

        public override void Execute()
        {
            EntityReference crmRecord = _crmContext.RecordCache[_alias];

            ProcessStage actualStage = GetCurrentStage(crmRecord);
            Guid? expectedStageId = GetStageByName(actualStage.ProcessId, _expectedStage);
            Assert.AreEqual(expectedStageId, actualStage.StageId);
        }

        private ProcessStage GetCurrentStage(EntityReference crmRecord)
        {
            var instance = BusinessProcessFlowHelper.GetProcessInstanceOfRecord(_crmContext, crmRecord);

            return new ProcessStage
            {
                ProcessId = instance.GetAttributeValue<EntityReference>("processid").Id,
                StageId = instance.GetAttributeValue<Guid>("processstageid")
            };
        }

        private Guid? GetStageByName(Guid processId, string stageName)
        {
            QueryExpression qe = new QueryExpression("processstage");
            qe.ColumnSet = new ColumnSet(true);
            qe.Criteria.AddCondition("processid", ConditionOperator.Equal, processId);
            qe.Criteria.AddCondition("stagename", ConditionOperator.Equal, stageName);
            qe.TopCount = 1;

            return GlobalTestingContext.ConnectionManager.CurrentConnection.RetrieveMultiple(qe)?.Entities?.FirstOrDefault()?.Id;
        }
    }
}
