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
        private readonly Dictionary<string, EntityReference> _aliasedRecords;
        private readonly List<string> _entitiesToIgnore;
        private readonly List<string> _aliasesToIgnore;
        private readonly ConnectionManager _connectionManager;
        private readonly MetadataCache _metadataCache;

        internal AliasedRecordCache(ConnectionManager connectionManager, MetadataCache metadataCache)
        {
            _aliasedRecords = new Dictionary<string, EntityReference>();
            _entitiesToIgnore = new List<string>();
            _aliasesToIgnore = new List<string>();
            _connectionManager = connectionManager;
            _metadataCache = metadataCache;
        }

        /// <summary>
        /// Adds a new record to the cache
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="reference">EntityReference of the record</param>
        public void Add(string alias, EntityReference reference, bool deleteRecordOnCleanup = true)
        {
            if(string.IsNullOrEmpty(reference.Name))
            {
                var md = _metadataCache.GetEntityMetadata(reference.LogicalName);
                var entity = _connectionManager.CurrentConnection.Retrieve(reference, new ColumnSet(md.PrimaryNameAttribute));
                reference.Name = entity.GetAttributeValue<string>(md.PrimaryNameAttribute);
            }

            Logger.WriteLine($"Adding alias {alias} to cache. {reference?.Id}");
            _aliasedRecords.Add(alias, reference);

            if (!deleteRecordOnCleanup)
                DoNotDeleteAlias(alias);
        }

        /// <summary>
        /// Adds a new record to the cache
        /// </summary>
        /// <param name="alias">Alias of the record. The record can be retrieved by this alias after it had been added. </param>
        /// <param name="entity">The record to add</param>
        public void Add(string alias, Entity entity, bool deleteRecordOnCleanup = true)
        {
            var md = _metadataCache.GetEntityMetadata(entity.LogicalName);
            Add(alias, entity.ToEntityReference(md.PrimaryNameAttribute), deleteRecordOnCleanup);
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

            Logger.WriteLine($"Getting Alias {alias} from cache. Result: {value?.Id}");
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
            {
                Logger.WriteLine($"Adding {alias} to cache with ID {entityReference?.Id}");
                _aliasedRecords[alias] = entityReference;
            }
            else
            {
                Logger.WriteLine($"Updating {alias} to cache with ID {entityReference?.Id}");
                _aliasedRecords.Add(alias, entityReference);
            }
        }

        /// <summary>
        /// Removes a record from the cache
        /// </summary>
        /// <param name="alias">Alias of the record</param>
        public void Remove(string alias)
        {
            Logger.WriteLine($"Removing {alias} from cache");
            _aliasedRecords.Remove(alias);
        }

        /// <summary>
        /// Deletes are records from the cache and deletes them in CRM
        /// </summary>
        /// <param name="service"></param>
        public void DeleteAllCachedRecords(CrmService service)
        {
            Logger.WriteLine("Clearing cache");
            foreach (var record in _aliasedRecords)
            {
                if (_aliasesToIgnore.Contains(record.Key) || _entitiesToIgnore.Contains(record.Value.LogicalName))
                    continue;

                try
                {
                    Logger.WriteLine($"Deleting {record.Key}. ID: {record.Value.Id} Entity: {record.Value.LogicalName}");
                    service.Delete(record.Value);
                }
                // Some records can't be deleted due to cascading behavior
                catch(Exception ex)
                {
                    Logger.WriteLine($"Delete failed: Error: {ex.Message}");
                }
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
