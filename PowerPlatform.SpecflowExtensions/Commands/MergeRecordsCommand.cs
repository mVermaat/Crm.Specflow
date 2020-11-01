using BoDi;
using FluentAssertions;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class MergeRecordsCommand : ApiOnlyCommand
    {
        private static readonly string[] _supportedMergeRecordTypes = new string[] { "account", "contact", "lead", "incident" };
        private static readonly string[] _unsupportedAttributes = new string[] { "ownerid" };

        private readonly EntityReference _targetRecord;
        private readonly EntityReference _subordinateRecord;
        private readonly Table _fieldsToPush;
        private readonly bool _mergeAll;

        public MergeRecordsCommand(IObjectContainer container, EntityReference targetAlias, EntityReference subordindateAlias) : base(container)
        {
            _targetRecord = targetAlias;
            _subordinateRecord = subordindateAlias;
            _mergeAll = true;
        }

        public MergeRecordsCommand(IObjectContainer container, EntityReference targetAlias, EntityReference subordindateAlias, Table mergeFields) : base(container)
        {
            _targetRecord = targetAlias;
            _subordinateRecord = subordindateAlias;
            _fieldsToPush = mergeFields;
        }

        public override void Execute()
        {
            VerifyRecordTypes();
            Logger.WriteLine($"Merging records of type {_targetRecord.LogicalName}");

            GlobalContext.ConnectionManager.CurrentCrmService.Execute<MergeResponse>(new MergeRequest
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

            Entity result = GlobalContext.ConnectionManager.CurrentCrmService.Retrieve(_subordinateRecord, columnSet);

            var attributeMetadata = GlobalContext.Metadata.GetEntityMetadata(_subordinateRecord.LogicalName).Attributes.ToDictionary(a => a.LogicalName);
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
            foreach (var remainingAttribute in result.Attributes)
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
            _targetRecord.LogicalName.Should().Be(_subordinateRecord.LogicalName, "Entity types of records to merge must be the same");
            _supportedMergeRecordTypes.Contains(_targetRecord.LogicalName).Should().BeTrue($"You are trying to merge a {_targetRecord.LogicalName}, but only {string.Join(", ", _supportedMergeRecordTypes)} are supported");
        }
    }
}
