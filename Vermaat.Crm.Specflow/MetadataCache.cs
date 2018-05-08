using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public class MetadataCache
    {
        private CrmService _service;

        private Dictionary<string, EntityMetadata> _cache;

        public MetadataCache(CrmService service)
        {
            _service = service;
            _cache = new Dictionary<string, EntityMetadata>();
        }

        public AttributeMetadata GetAttributeMetadata(string entityName, string attributeName)
        {
            var entityData = GetEntityMetadata(entityName);

            var md = entityData.Attributes.Where(a => a.LogicalName.Equals(attributeName)).FirstOrDefault();
            
            Assert.IsNotNull(md, string.Format("Can't find attribute {0} on entity {1}", attributeName, entityName));

            return md;
        }

        public EntityMetadata GetEntityMetadata(string entityName)
        {
            if(!_cache.TryGetValue(entityName, out EntityMetadata result))
            {
                var req = new RetrieveEntityRequest()
                {
                    EntityFilters = EntityFilters.Attributes,
                    RetrieveAsIfPublished = true,
                    LogicalName = entityName,
                };

                result = _service.Execute<RetrieveEntityResponse>(req).EntityMetadata;
                _cache.Add(entityName, result);
            }
            return result;
        }
    }
}
