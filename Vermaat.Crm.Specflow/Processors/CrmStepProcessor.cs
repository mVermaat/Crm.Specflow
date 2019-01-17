using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Processors
{
    public class CrmStepProcessor : ICrmStepProcessor
    {
        protected CrmTestingContext CrmContext { get; private set; }

        public CrmStepProcessor(CrmTestingContext crmContext)
        {
            CrmContext = crmContext;
        }

        public virtual DataCollection<Entity> GetRecords(string entityLogicalName, Table table)
        {
            var query = QueryHelper.CreateQueryExpressionFromTable(entityLogicalName, table, CrmContext);
            return HelperMethods.ExecuteWithRetry(20, 500, () => CrmContext.Service.RetrieveMultiple(query)).Entities;
        }

        public virtual void CreateAliasedRecord(string entityLogicalName, Table table, string alias)
        {
            Entity toCreate = CrmContext.RecordBuilder.SetupEntityWithDefaults(entityLogicalName, table);
            CrmContext.Service.Create(toCreate, alias);
        }

        public virtual void UpdateRecord(EntityReference crmRecord, Table table)
        {
            Entity toUpdate = new Entity(crmRecord.LogicalName)
            {
                Id = crmRecord.Id
            };

            foreach (var row in table.Rows)
            {
                toUpdate[row["Property"]] = ObjectConverter.ToCrmObject(crmRecord.LogicalName, row["Property"], row["Value"], CrmContext);
            }

            CrmContext.Service.Update(toUpdate);
        }

        public virtual void UpdateStatus(EntityReference crmRecord, string newStatusCode)
        {
            var request = ObjectConverter.ToSetStateRequest(crmRecord, newStatusCode, CrmContext);
            CrmContext.Service.Execute<SetStateResponse>(request);
        }

        public virtual void DeleteRecord(EntityReference crmRecord)
        {
            CrmContext.Service.Delete(crmRecord);
        }

        public virtual void AssignRecord(EntityReference assignTo, EntityReference recordToAssign)
        {
            AssignRequest req = new AssignRequest()
            {
                Assignee = assignTo,
                Target = recordToAssign
            };
            CrmContext.Service.Execute<AssignResponse>(req);
        }

        public virtual void AssertRecordHasValues(EntityReference crmRecord, Table table)
        {
            var columns = new ColumnSet(table.Rows.Select(r => r["Property"]).ToArray());
            var record = CrmContext.Service.Retrieve(crmRecord, columns);
            AssertHelper.HasProperties(record, table, CrmContext);
        }
    }
}
