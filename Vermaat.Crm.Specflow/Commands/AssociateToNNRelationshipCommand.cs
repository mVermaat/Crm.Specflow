using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Commands
{
    public class AssociateToNNRelationshipCommand : ApiOnlyCommand
    {
        private readonly string _alias;
        private readonly string _relatedEntityName;
        private readonly Table _recordsToConnect;

        public AssociateToNNRelationshipCommand(CrmTestingContext crmContext, string alias, string relatedEntityName, Table recordsToConnect) : base(crmContext)
        {
            _alias = alias;
            _relatedEntityName = relatedEntityName;
            _recordsToConnect = recordsToConnect;
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

            EntityReferenceCollection toAssociate = new EntityReferenceCollection();
            foreach (var row in _recordsToConnect.Rows)
            {
                var lookupValue = ObjectConverter.GetLookupValue(_crmContext, row[Constants.SpecFlow.TABLE_VALUE], _relatedEntityName);
                if (lookupValue == null)
                    throw new TestExecutionException(Constants.ErrorCodes.RECORD_NOT_FOUND, row[Constants.SpecFlow.TABLE_VALUE], _relatedEntityName);
                toAssociate.Add(lookupValue);
            }

            Logger.WriteLine($"Associating {toAssociate.Count} records");
            GlobalTestingContext.ConnectionManager.CurrentConnection.Associate(record.LogicalName, record.Id, new Relationship(relationship.SchemaName), toAssociate);

        }
    }
}
