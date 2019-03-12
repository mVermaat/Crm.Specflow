using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    /// <summary>
    /// Cache of all records that were created in the specflow scenario
    /// </summary>
    public class AliasedRecordCache
    {
        private Dictionary<string, EntityReference> _aliasedRecords;
        private List<string> _entitiesToIgnore;
        private List<string> _aliasesToIgnore;
        private readonly CrmService _service;
        private readonly MetadataCache _metadataCache;

        internal AliasedRecordCache(CrmService service, MetadataCache metadataCache)
        {
            _aliasedRecords = new Dictionary<string, EntityReference>();
            _entitiesToIgnore = new List<string>();
            _aliasesToIgnore = new List<string>();
            _service = service;
            _metadataCache = metadataCache;
        }

        /// <summary>
        /// Adds a new record to the cache
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="reference">EntityReference of the record</param>
        public void Add(string alias, EntityReference reference)
        {
            if(string.IsNullOrEmpty(reference.Name))
            {
                var md = _metadataCache.GetEntityMetadata(reference.LogicalName);
                var entity = _service.Retrieve(reference, new ColumnSet(md.PrimaryNameAttribute));
                reference.Name = entity.GetAttributeValue<string>(md.PrimaryNameAttribute);
            }

            _aliasedRecords.Add(alias, reference);
        }

        /// <summary>
        /// Adds a new record to the cache
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="entity">The record to add</param>
        public void Add(string alias, Entity entity)
        {
            var md = _metadataCache.GetEntityMetadata(entity.LogicalName);
            Add(alias, entity.ToEntityReference(md.PrimaryNameAttribute));
        }

        /// <summary>
        /// Retrieves a record from the cache
        /// </summary>
        /// <param name="alias">Alias of the record to retrieve</param>
        /// <param name="mustExist">If set to true, the test fails if it doesn't exist. If set to false, it returns null if it doesn't exist</param>
        /// <returns></returns>
        public EntityReference Get(string alias, bool mustExist = false)
        {
            if(!_aliasedRecords.TryGetValue(alias, out EntityReference value) && mustExist)
                Assert.Fail("alias {0} doesn't exist", alias);

            return value;
        }

        /// <summary>
        /// Adds the record to the cache if it doesn't exist. Overwrites it if it does
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="entity">EntityReference of the record</param>
        public void Upsert(string alias, Entity entity)
        {
            Upsert(alias, entity.ToEntityReference());
        }

        /// <summary>
        /// Adds the record to the cache if it doesn't exist. Overwrites it if it does
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="reference">EntityReference of the record</param>
        public void Upsert(string alias, EntityReference entityReference)
        {
            if (_aliasedRecords.ContainsKey(alias))
                _aliasedRecords[alias] = entityReference;
            else
                _aliasedRecords.Add(alias, entityReference);
        }

        /// <summary>
        /// Removes a record from the cache
        /// </summary>
        /// <param name="alias">Alias of the record</param>
        public void Remove(string alias)
        {
            _aliasedRecords.Remove(alias);
        }

        /// <summary>
        /// Deletes are records from the cache and deletes them in CRM
        /// </summary>
        /// <param name="service"></param>
        public void DeleteAllCachedRecords(CrmService service)
        {
            foreach (var record in _aliasedRecords)
            {
                if (_aliasesToIgnore.Contains(record.Key) || _entitiesToIgnore.Contains(record.Value.LogicalName))
                    continue;

                try
                {
                    service.Delete(record.Value);
                }
                // Some records can be deleted due to cascading behavior
                catch { }
            }
            _aliasedRecords.Clear();
        }

        /// <summary>
        /// Sets a logical name of an entity that shouldn't be deleted from CRM when DeleteAllCachedRecords is called
        /// </summary>
        /// <param name="entityName">LogicalName of the entity</param>
        public void DoNotDeleteAnyRecordsOfEntity(string entityName)
        {
            if (!_entitiesToIgnore.Contains(entityName))
                _entitiesToIgnore.Add(entityName);
        }

        /// <summary>
        /// Sets that a specific alias shouldn't be deleted from CRM when DeleteAllCachedRecords is called
        /// </summary>
        /// <param name="alias"></param>
        public void DoNotDeleteAlias(string alias)
        {
            if (!_aliasesToIgnore.Contains(alias))
                _aliasesToIgnore.Add(alias);
        }

        /// <summary>
        /// Retrieves a record from the cache
        /// </summary>
        /// <param name="alias">Name of the record</param>
        /// <returns></returns>
        public EntityReference this[string alias]
        {
            get { return Get(alias, true); }
            set { Upsert(alias, value); }
        }
    }
}
