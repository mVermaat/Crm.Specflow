using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace Vermaat.Crm.Specflow
{
    public static class BusinessProcessFlowHelper
    {
        public static Entity GetProcessRecord(Entity instance)
        {
            var process = GlobalTestingContext.ConnectionManager.CurrentConnection.Retrieve(
                instance.GetAttributeValue<EntityReference>("processid"), new ColumnSet("uniquename"));

            return GlobalTestingContext.ConnectionManager.CurrentConnection.Retrieve($"{process.GetAttributeValue<string>("uniquename")}", instance.Id, new ColumnSet("activestageid"));
        }

        public static Entity[] GetActivePath(CrmTestingContext crmContext, Entity instance)
        {
            var req = new RetrieveActivePathRequest()
            {
                ProcessInstanceId = instance.Id
            };
            return GlobalTestingContext.ConnectionManager.CurrentConnection.Execute<RetrieveActivePathResponse>(req).ProcessStages.Entities.ToArray();
        }

        public static Entity GetProcessInstanceOfRecord(CrmTestingContext crmContext, EntityReference crmRecord)
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
            var response = GlobalTestingContext.ConnectionManager.CurrentConnection.Execute<RetrieveProcessInstancesResponse>(request);
            return response.Processes.Entities.FirstOrDefault();
        }
    }
}
