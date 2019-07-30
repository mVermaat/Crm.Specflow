using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Commands
{
    public class AssertNNRelationshipCommand : ApiOnlyCommand
    {
        private readonly string _alias;
        private readonly string _relatedEntityName;
        private readonly Table _expectedRecords;

        public AssertNNRelationshipCommand(CrmTestingContext crmContext, string alias, string relatedEntityName, Table expectedRecords) : base(crmContext)
        {
            _alias = alias;
            _relatedEntityName = relatedEntityName;
            _expectedRecords = expectedRecords;
        }

        public override void Execute()
        {
            var record = _crmContext.RecordCache.Get(_alias);
            var md = GlobalTestingContext.Metadata.GetEntityMetadata(record.LogicalName, EntityFilters.Relationships);

            var relationship = md.ManyToManyRelationships.FirstOrDefault(r => (r.Entity1LogicalName == record.LogicalName && r.Entity2LogicalName == _relatedEntityName) ||
                                                           (r.Entity1LogicalName == _relatedEntityName && r.Entity2LogicalName == record.LogicalName));

            if (relationship == null)
                throw new TestExecutionException(Constants.ErrorCodes.N_N_RELATIONSHIP_NOT_FOUND, record.LogicalName, _relatedEntityName);

            Logger.WriteLine($"Using relationship {relationship.SchemaName}");

            EntityReferenceCollection records = new EntityReferenceCollection();
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
            var result = GlobalTestingContext.ConnectionManager.CurrentConnection.RetrieveMultiple(query);

            Assert.AreEqual(records.Count, result.Entities.Count, $"Different records: {string.Join(", ", records.Where(r => !result.Entities.Select(e => e.GetAttributeValue<Guid>(relatedFieldName)).Contains(r.Id)).Select(r => r.Name))}");
        }
    }
}
