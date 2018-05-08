using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public static class BusinessProcessHelper
    {
        public static Entity GetProcessInstanceOfRecord(EntityReference crmRecord, CrmService service)
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
            var response = service.Execute<RetrieveProcessInstancesResponse>(request);
            return response.Processes.Entities.FirstOrDefault();
        }

        public static Entity GetStageByName(EntityReference processId, string name, CrmService service)
        {
            QueryExpression qe = new QueryExpression("processstage");
            qe.ColumnSet = new ColumnSet(true);
            qe.Criteria.AddCondition("processid", ConditionOperator.Equal, processId.Id);
            qe.Criteria.AddCondition("stagename", ConditionOperator.Equal, name);
            qe.TopCount = 1;

            return service.RetrieveMultiple(qe)?.Entities?.FirstOrDefault();
        }

        public static void MoveToNextStage(EntityReference crmRecord, CrmService service)
        {
            var instance = GetProcessInstanceOfRecord(crmRecord, service);
            var path = GetActivePath(instance, service);

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

            var processRecord = GetProcessRecord(crmRecord, instance.Id, service);
            processRecord["activestageid"] = path[currentStage + 1].ToEntityReference();
            service.Update(processRecord);
        }

        public static void MoveToStage(EntityReference crmRecord, string stageName, CrmService service)
        {
            var instance = GetProcessInstanceOfRecord(crmRecord, service);
            var path = GetActivePath(instance, service);

            int currentStage = -1;
            int desiredStage = -1;

            for(int i = 0; i < path.Length && (currentStage == -1 || desiredStage == -1); i++)
            {
                if(stageName == path[i].GetAttributeValue<string>("stagename"))
                    desiredStage = i;
                if(path[i].Id == instance.GetAttributeValue<Guid>("processstageid"))
                    currentStage = i;
            }

            if (currentStage == -1)
                throw new InvalidOperationException("Current stage can't be found");
            if (desiredStage == -1)
                throw new InvalidOperationException($"{stageName} isn't in the active path");

            if (currentStage == desiredStage)
                return;

            var processRecord = GetProcessRecord(crmRecord, instance.Id, service);

            if (desiredStage < currentStage)
            {
                processRecord["activestageid"] = path[desiredStage].ToEntityReference();
                service.Update(processRecord);
                return;
            }

            while (desiredStage > currentStage)
            {
                processRecord["activestageid"] = path[currentStage + 1].ToEntityReference();
                service.Update(processRecord);
                currentStage++;
            }

           




        }

        private static Entity GetProcessRecord(EntityReference crmRecord, Guid instanceId, CrmService service)
        {
            return service.Retrieve($"{crmRecord.LogicalName}process", instanceId, new ColumnSet("activestageid"));
        }

        private static Entity[] GetActivePath(Entity instance, CrmService service)
        {
            var req = new RetrieveActivePathRequest()
            {
                ProcessInstanceId = instance.Id
            };
            return service.Execute<RetrieveActivePathResponse>(req).ProcessStages.Entities.ToArray();
        }
    }
}
