using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions
{
    public class MetadataCache
    {

        private readonly Dictionary<string, Dictionary<EntityFilters, EntityMetadata>> _entityMetadataCache;
        private readonly Dictionary<string, DataCollection<Entity>> _attributeMapCache;
        private readonly Dictionary<string, Guid> _formCache;
        private Dictionary<string, ModelApp> _modelAppCache;

        public MetadataCache()
        {
            _entityMetadataCache = new Dictionary<string, Dictionary<EntityFilters, EntityMetadata>>();
            _attributeMapCache = new Dictionary<string, DataCollection<Entity>>();
            _formCache = new Dictionary<string, Guid>();
        }

        /// <summary>
        /// Gets the Metadata for an attribute based on the logicalname
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="logicalName"></param>
        /// <returns></returns>
        public AttributeMetadata GetAttributeMetadata(string entityName, string logicalName)
        {
            var entityData = GetEntityMetadata(entityName);
            var md = entityData.Attributes.Where(a => a.LogicalName.Equals(logicalName)).FirstOrDefault();

            if (md == null)
                throw new TestExecutionException(Constants.ErrorCodes.ATTRIBUTE_DOESNT_EXIST, logicalName, entityName);
            
            return md;
        }

        /// <summary>
        ///  Gets the metadata for an attibute based on the display name in a specific language
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="displayName"></param>
        /// <param name="languageCode">CRM language code to look for the display name</param>
        /// <returns></returns>
        public AttributeMetadata GetAttributeMetadata(string entityName, string displayName, int languageCode)
        {
            var entityMd = GetEntityMetadata(entityName);

            var attributeMd = entityMd.Attributes.Where(a => a.DisplayName.IsLabel(languageCode, displayName) || a.LogicalName.Equals(displayName)).ToArray();


            if (attributeMd.Length == 0)
                throw new TestExecutionException(Constants.ErrorCodes.ATTRIBUTE_DOESNT_EXIST, displayName, entityName);
            else if (attributeMd.Length > 1)
                throw new TestExecutionException(Constants.ErrorCodes.MULTIPLE_ATTRIBUTES_FOUND, displayName, string.Join(", ", attributeMd.Select(md => md.LogicalName)));

            return attributeMd.First();

        }

        /// <summary>
        /// Gets the metadata for a specific entity
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public EntityMetadata GetEntityMetadata(string entityName, EntityFilters filters = EntityFilters.Attributes)
        {
            if (!_entityMetadataCache.TryGetValue(entityName, out var metadataDic))
            {
                metadataDic = new Dictionary<EntityFilters, EntityMetadata>();
                _entityMetadataCache.Add(entityName, metadataDic);
            }

            if (!metadataDic.TryGetValue(filters, out EntityMetadata result))
            {
                var req = new RetrieveEntityRequest()
                {
                    EntityFilters = filters,
                    RetrieveAsIfPublished = true,
                    LogicalName = entityName,
                };

                result = GlobalContext.ConnectionManager.AdminConnection.Execute<RetrieveEntityResponse>(req).EntityMetadata;
                metadataDic.Add(filters, result);
            }
            return result;
        }


        /// <summary>
        /// Geta all the data mappings between two entities
        /// </summary>
        /// <param name="parentEntity"></param>
        /// <param name="childEntity"></param>
        /// <returns></returns>
        public DataCollection<Entity> GetAttributeMaps(string parentEntity, string childEntity)
        {
            Logger.WriteLine($"Getting attribute maps between {parentEntity} and {childEntity}");
            if (!_attributeMapCache.TryGetValue(parentEntity + childEntity, out DataCollection<Entity> result))
            {
                Logger.WriteLine("Not cached yet. Retrieving from CRM");
                var query = new QueryExpression(AttributeMap.EntityLogicalName);
                query.ColumnSet.AddColumns(AttributeMap.Fields.SourceAttributeName, AttributeMap.Fields.TargetAttributeName);

                var link = query.AddLink(EntityMap.EntityLogicalName, AttributeMap.Fields.EntityMapId, EntityMap.Fields.Id);
                link.LinkCriteria.AddCondition(EntityMap.Fields.SourceEntityName, ConditionOperator.Equal, parentEntity);
                link.LinkCriteria.AddCondition(EntityMap.Fields.TargetEntityName, ConditionOperator.Equal, childEntity);

                result = GlobalContext.ConnectionManager.AdminConnection.RetrieveMultiple(query).Entities;
                _attributeMapCache.Add(parentEntity + childEntity, result);
            }
            return result;
        }

        public Guid GetFormId(string entityName, string formName)
        {
            if (!_formCache.TryGetValue($"{entityName}_{formName}", out Guid formId))
            {
                var query = new QueryExpression(SystemForm.EntityLogicalName)
                {
                    TopCount = 1
                };
                query.ColumnSet.AddColumn(SystemForm.Fields.FormId);
                query.Criteria.AddCondition(SystemForm.Fields.ObjectTypeCode, ConditionOperator.Equal, entityName);
                query.Criteria.AddCondition(SystemForm.Fields.Name, ConditionOperator.Equal, formName);

                var result = GlobalContext.ConnectionManager.AdminConnection.RetrieveMultiple(query).Entities.FirstOrDefault();

                if (result == null)
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_NOT_FOUND, formName, entityName);
                _formCache.Add($"{entityName}_{formName}", result.Id);
                formId = result.Id;
            }
            return formId;
        }

        public ModelApp GetModelApp(string appUniqueName)
        {
            if(_modelAppCache == null)
            {
                var query = new QueryExpression(ModelApp.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(ModelApp.Fields.UniqueName),
                    NoLock = true
                };

                var result = GlobalContext.ConnectionManager.AdminConnection.RetrieveMultiple(query);


                _modelAppCache = result.Entities.Select(e => new ModelApp(e)).ToDictionary(a => a.Name);

            }

            if(!_modelAppCache.TryGetValue(appUniqueName, out var modelApp))
            {
                throw new TestExecutionException(Constants.ErrorCodes.APP_NOT_FOUND, appUniqueName);
            }
            return modelApp;

        }
    }
}
