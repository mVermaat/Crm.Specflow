using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public class MetadataCache
    {
        private ConnectionManager _connectionManager;

        private Dictionary<string, Dictionary<EntityFilters, EntityMetadata>> _entityMetadataCache;
        private Dictionary<string, DataCollection<Entity>> _attributeMapCache;
        private Dictionary<string, Guid> _formCache;

        public MetadataCache(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            _entityMetadataCache = new Dictionary<string, Dictionary<EntityFilters, EntityMetadata>>();
            _attributeMapCache = new Dictionary<string, DataCollection<Entity>>();
            _formCache = new Dictionary<string, Guid>();
        }

        public AttributeMetadata GetAttributeMetadata(string entityName, string logicalName)
        {
            var entityData = GetEntityMetadata(entityName);

            var md = entityData.Attributes.Where(a => a.LogicalName.Equals(logicalName)).FirstOrDefault();
            
            Assert.IsNotNull(md, string.Format("Can't find attribute {0} on entity {1}", logicalName, entityName));

            return md;
        }

        public AttributeMetadata GetAttributeMetadata(string entityName, string displayName, int languageCode)
        {
            var entityMd = GetEntityMetadata(entityName);

            var attributeMd = entityMd.Attributes.Where(a => a.DisplayName.IsLabel(languageCode, displayName) || a.LogicalName.Equals(displayName)).FirstOrDefault();

            if (attributeMd == null)
                throw new TestExecutionException(Constants.ErrorCodes.ATTRIBUTE_DOESNT_EXIST, displayName, entityName);

            return attributeMd;

        }

        public EntityMetadata GetEntityMetadata(string entityName, EntityFilters filters = EntityFilters.Attributes)
        {
            if (!_entityMetadataCache.TryGetValue(entityName, out var metadataDic))
            {
                metadataDic = new Dictionary<EntityFilters, EntityMetadata>();
                _entityMetadataCache.Add(entityName, metadataDic);
            }

            if(!metadataDic.TryGetValue(filters, out EntityMetadata result))
            { 
                var req = new RetrieveEntityRequest()
                {
                    EntityFilters = filters,
                    RetrieveAsIfPublished = true,
                    LogicalName = entityName,
                };

                result = _connectionManager.CurrentConnection.Execute<RetrieveEntityResponse>(req).EntityMetadata;
                metadataDic.Add(filters, result);
            }
            return result;
        }

        public DataCollection<Entity> GetAttributeMaps(string parentEntity, string childEntity)
        {
            Logger.WriteLine($"Getting attribute maps between {parentEntity} and {childEntity}");
            if(!_attributeMapCache.TryGetValue(parentEntity+childEntity, out DataCollection<Entity> result)) 
            {
                Logger.WriteLine("Not cached yet. Retrieving from CRM");
                var query = new QueryExpression("attributemap");
                query.ColumnSet.AddColumns("sourceattributename", "targetattributename");

                var link = query.AddLink("entitymap", "entitymapid", "entitymapid");
                link.LinkCriteria.AddCondition("sourceentityname", ConditionOperator.Equal, parentEntity);
                link.LinkCriteria.AddCondition("targetentityname", ConditionOperator.Equal, childEntity);

                result = GlobalTestingContext.ConnectionManager.CurrentConnection.RetrieveMultiple(query).Entities;
                _attributeMapCache.Add(parentEntity + childEntity, result);
            }
            return result;
        }

        public Guid GetFormId(string entityName, string formName)
        {
            if(!_formCache.TryGetValue($"{entityName}_{formName}", out Guid formId))
            {
                var query = new QueryExpression("systemform");
                query.TopCount = 1;
                query.ColumnSet.AddColumn("formid");
                query.Criteria.AddCondition("objecttypecode", ConditionOperator.Equal, entityName);
                query.Criteria.AddCondition("name", ConditionOperator.Equal, formName);

                var result = GlobalTestingContext.ConnectionManager.CurrentConnection.RetrieveMultiple(query).Entities.FirstOrDefault();

                if (result == null)
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_NOT_FOUND, formName, entityName);
                _formCache.Add($"{entityName}_{formName}", result.Id);
                formId = result.Id;
            }
            return formId;
        }
    }
}
