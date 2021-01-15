using BoDi;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
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
    public class AssertNNRelationshipCommand : ApiOnlyCommand
    {
        private readonly string _alias;
        private readonly string _relatedEntityName;
        private readonly Table _expectedRecords;

        public AssertNNRelationshipCommand(IObjectContainer container, string alias, string relatedEntityName, Table expectedRecords) 
            : base(container)
        {
            _alias = alias;
            _relatedEntityName = relatedEntityName;
            _expectedRecords = expectedRecords;
        }

        public override void Execute()
        {
            var record = _crmContext.RecordCache.Get(_alias);
            var md = GlobalContext.Metadata.GetEntityMetadata(record.LogicalName, EntityFilters.Relationships);

            var relationship = md.ManyToManyRelationships.FirstOrDefault(r => (r.Entity1LogicalName == record.LogicalName && r.Entity2LogicalName == _relatedEntityName) ||
                                                           (r.Entity1LogicalName == _relatedEntityName && r.Entity2LogicalName == record.LogicalName));

            if (relationship == null)
                throw new TestExecutionException(Constants.ErrorCodes.N_N_RELATIONSHIP_NOT_FOUND, record.LogicalName, _relatedEntityName);

            Logger.WriteLine($"Using relationship {relationship.SchemaName}");

            var records = new EntityReferenceCollection();
            foreach (var row in _expectedRecords.Rows)
            {
                var lookupValue = ObjectConverter.GetLookupValue(_crmContext, row[Constants.SpecFlow.TABLE_VALUE], _relatedEntityName);
                if (lookupValue == null)
                    throw new TestExecutionException(Constants.ErrorCodes.RECORD_NOT_FOUND, row[Constants.SpecFlow.TABLE_VALUE], _relatedEntityName);
                records.Add(lookupValue);
            }

            var currentRecordFieldName = relationship.Entity1LogicalName == record.LogicalName ? relationship.Entity1IntersectAttribute : relationship.Entity2IntersectAttribute;
            var relatedFieldName = relationship.Entity1LogicalName == _relatedEntityName ? relationship.Entity1IntersectAttribute : relationship.Entity2IntersectAttribute;

            var query = new QueryExpression(relationship.IntersectEntityName);
            query.ColumnSet.AddColumn(relatedFieldName);
            query.Criteria.AddCondition(currentRecordFieldName, ConditionOperator.Equal, record.Id);
            query.Criteria.AddCondition(relatedFieldName, ConditionOperator.In, records.Select(r => (object)r.Id).ToArray());
            var result = GlobalContext.ConnectionManager.CurrentCrmService.RetrieveMultiple(query);

            records.Should().HaveCount(result.Entities.Count, $"Different records: {string.Join(", ", records.Where(r => !result.Entities.Select(e => e.GetAttributeValue<Guid>(relatedFieldName)).Contains(r.Id)).Select(r => r.Name))}");
        }
    }
}
