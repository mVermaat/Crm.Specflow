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
    static class BusinessProcessFlowHelper
    {
        public static Entity GetProcessRecord(CrmTestingContext crmContext, EntityReference crmRecord, Guid instanceId)
        {
            return GlobalTestingContext.ConnectionManager.CurrentConnection.Retrieve($"{crmRecord.LogicalName}process", instanceId, new ColumnSet("activestageid"));
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
