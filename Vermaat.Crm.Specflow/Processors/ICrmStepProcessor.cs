using Microsoft.Xrm.Sdk;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Processors
{
    public interface ICrmStepProcessor
    {
        void AssertRecordHasValues(EntityReference crmRecord, Table table);
        void AssignRecord(EntityReference assignTo, EntityReference recordToAssign);
        void CreateAliasedRecord(string entityLogicalName, Table table, string alias);
        void DeleteRecord(EntityReference crmRecord);
        DataCollection<Entity> GetRecords(string entityLogicalName, Table table);
        void UpdateRecord(EntityReference crmRecord, Table table);
        void UpdateStatus(EntityReference crmRecord, string newStatusCode);
    }
}