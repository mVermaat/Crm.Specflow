using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Processors
{
    public class BusinessProcessFlowProcessor : IBusinessProcessFlowProcessor
    {
        private readonly CrmTestingContext _crmContext;

        public BusinessProcessFlowProcessor(CrmTestingContext context)
        {
            this._crmContext = context;
        }

        public void MoveToStage(EntityReference crmRecord, string stageName)
        {
            var instance = GetProcessInstanceOfRecord(crmRecord);
            var path = GetActivePath(instance);

            int currentStage = -1;
            int desiredStage = -1;

            for (int i = 0; i < path.Length && (currentStage == -1 || desiredStage == -1); i++)
            {
                if (stageName == path[i].GetAttributeValue<string>("stagename"))
                    desiredStage = i;
                if (path[i].Id == instance.GetAttributeValue<Guid>("processstageid"))
                    currentStage = i;
            }

            if (currentStage == -1)
                throw new InvalidOperationException("Current stage can't be found");
            if (desiredStage == -1)
                throw new InvalidOperationException($"{stageName} isn't in the active path");

            if (currentStage == desiredStage)
                return;

            var processRecord = GetProcessRecord(crmRecord, instance.Id);

            if (desiredStage < currentStage)
            {
                processRecord["activestageid"] = path[desiredStage].ToEntityReference();
                _crmContext.Service.Update(processRecord);
                return;
            }

            while (desiredStage > currentStage)
            {
                processRecord["activestageid"] = path[currentStage + 1].ToEntityReference();
                _crmContext.Service.Update(processRecord);
                currentStage++;
            }
        }

        public void MoveNext(EntityReference crmRecord)
        {
            var instance = GetProcessInstanceOfRecord(crmRecord);
            var path = GetActivePath(instance);

            var currentStage = -1;
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

            var processRecord = GetProcessRecord(crmRecord, instance.Id);
            processRecord["activestageid"] = path[currentStage + 1].ToEntityReference();
            _crmContext.Service.Update(processRecord);
        }

        public ProcessStage GetCurrentStage(EntityReference aliasRef)
        {
            var instance = GetProcessInstanceOfRecord(aliasRef);

            return new ProcessStage
            {
                ProcessId = instance.GetAttributeValue<EntityReference>("processid").Id,
                StageId = instance.GetAttributeValue<Guid>("processstageid")
            };
        }

        public Guid? GetStageByName(Guid processId, string stageName)
        {
            QueryExpression qe = new QueryExpression("processstage");
            qe.ColumnSet = new ColumnSet(true);
            qe.Criteria.AddCondition("processid", ConditionOperator.Equal, processId);
            qe.Criteria.AddCondition("stagename", ConditionOperator.Equal, stageName);
            qe.TopCount = 1;

            return _crmContext.Service.RetrieveMultiple(qe)?.Entities?.FirstOrDefault()?.Id;
        }

        private Entity GetProcessRecord(EntityReference crmRecord, Guid instanceId)
        {
            return _crmContext.Service.Retrieve($"{crmRecord.LogicalName}process", instanceId, new ColumnSet("activestageid"));
        }

        private Entity[] GetActivePath(Entity instance)
        {
            var req = new RetrieveActivePathRequest()
            {
                ProcessInstanceId = instance.Id
            };
            return _crmContext.Service.Execute<RetrieveActivePathResponse>(req).ProcessStages.Entities.ToArray();
        }

        private Entity GetProcessInstanceOfRecord(EntityReference crmRecord)
        {
            if (crmRecord == null)
            {
                return null;
            }

            var request = new RetrieveProcessInstancesRequest()
            {
                EntityId = crmRecord.Id,
                EntityLogicalName = crmRecord.LogicalName
            };
            var response = _crmContext.Service.Execute<RetrieveProcessInstancesResponse>(request);
            return response.Processes.Entities.FirstOrDefault();
        }
    }
}
