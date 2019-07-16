using System.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Commands
{
    public class MergeRecordsCommand : ApiOnlyCommand
    {
        private static readonly string[] _supportedMergeRecordTypes = new string[] { "account", "contact", "lead", "incident" };
        private static readonly string[] _unsupportedAttributes = new string[] { "ownerid" };

        private readonly EntityReference _targetRecord;
        private readonly EntityReference _subordinateRecord;
        private readonly Table _fieldsToPush;
        private readonly bool _mergeAll;

        public MergeRecordsCommand(CrmTestingContext crmContext, EntityReference targetAlias, EntityReference subordindateAlias) : base(crmContext)
        {
            _targetRecord = targetAlias;
            _subordinateRecord = subordindateAlias;
            _mergeAll = true;
        }

        public MergeRecordsCommand(CrmTestingContext crmContext, EntityReference targetAlias, EntityReference subordindateAlias, Table mergeFields) : base(crmContext)
        {
            _targetRecord = targetAlias;
            _subordinateRecord = subordindateAlias;
            _fieldsToPush = mergeFields;
        }

        public override void Execute()
        {
            VerifyRecordTypes();
            Logger.WriteLine($"Merging records of type {_targetRecord.LogicalName}");

            GlobalTestingContext.ConnectionManager.CurrentConnection.Execute<MergeResponse>(new MergeRequest
            {
                PerformParentingChecks = false,
                SubordinateId = _subordinateRecord.Id,
                Target = _targetRecord,
                UpdateContent = BuildChangeEntity()
            });
            Logger.WriteLine("Merge successful");
        }

        private Entity BuildChangeEntity()
        {
            ColumnSet columnSet = _mergeAll ?
                new ColumnSet(true) :
                new ColumnSet(_fieldsToPush.Rows.Select(r => r[Constants.SpecFlow.TABLE_KEY]).ToArray());

            Entity result = GlobalTestingContext.ConnectionManager.CurrentConnection.Retrieve(_subordinateRecord, columnSet);

            var attributeMetadata = GlobalTestingContext.Metadata.GetEntityMetadata(_subordinateRecord.LogicalName).Attributes.ToDictionary(a => a.LogicalName);
            var deleteQuery = result.Attributes.Where(a =>
                attributeMetadata[a.Key].IsPrimaryId.GetValueOrDefault() || 
                !attributeMetadata[a.Key].IsValidForUpdate.GetValueOrDefault() ||
                _unsupportedAttributes.Contains(a.Key) ||
                (_mergeAll && a.Value == null));

            foreach (var toRemove in deleteQuery.ToArray())
            {
                result.Attributes.Remove(toRemove);
            }

            // Need to clear name from entityreference, as merge creates an error if an entityreference has its name filled.
            foreach(var remainingAttribute in result.Attributes)
            {
                var value = remainingAttribute.Value as EntityReference;
                if (value != null)
                    value.Name = null;
            }

            Logger.WriteLine($"Merging fields: {string.Join(", ", result.Attributes.Keys)}");

            return result;
        }

        private void VerifyRecordTypes()
        {
            Assert.AreEqual(_targetRecord.LogicalName, _subordinateRecord.LogicalName, "Entity types of records to merge must be the same");
            Assert.IsTrue(_supportedMergeRecordTypes.Contains(_targetRecord.LogicalName), $"You are trying to merge a {_targetRecord.LogicalName}, but only {string.Join(", ", _supportedMergeRecordTypes)} are supported");
        }
    }
}
