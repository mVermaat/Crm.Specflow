using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Query;

namespace Vermaat.Crm.Specflow
{
    /// <summary>
    /// Connection to Dynamics CRM
    /// </summary>
    public class CrmService : IOrganizationService
    {
        private readonly Lazy<IOrganizationService> _service;
        private readonly CrmConnectionString _connectionString;
        private IOrganizationService Service => _service.Value;

        public AliasedRecordCache RecordCache { get; set; }
        public ICrmConnectionProvider ConnectionProvider { get; set; }
        

        public CrmService(CrmConnectionString connectionString)
            : this(connectionString, new DefaultCrmConnectionProvider())
        {
           
        }

        public CrmService(CrmConnectionString connectionString, ICrmConnectionProvider provider)
        {
            _connectionString = connectionString;
            ConnectionProvider = provider;
            _service = new Lazy<IOrganizationService>(ConnectToCrm);
        }

        public void Create(Entity entity, string alias)
        {
            entity.Id = CreateRecord(entity);
            RecordCache.Add(alias, entity.ToEntityReference());
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            Service.Associate(entityName, entityId, relationship, relatedEntities);
        }

        public void Delete(string entityName, Guid id)
        {
            Service.Delete(entityName, id);
        }

        public void Delete(EntityReference entityReference)
        {
            Service.Delete(entityReference.LogicalName, entityReference.Id);
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            Service.Disassociate(entityName, entityId, relationship, relatedEntities);
        }

        public T Execute<T>(OrganizationRequest request) where T : OrganizationResponse
        {
            return (T)ExecuteRequest(request);
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            return Service.Retrieve(entityName, id, columnSet);
        }

        public Entity Retrieve(EntityReference entityReference, ColumnSet columnSet)
        {
            return Service.Retrieve(entityReference.LogicalName, entityReference.Id, columnSet);
        }

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            return Service.RetrieveMultiple(query);
        }

        public void Update(Entity entity)
        {
            Service.Update(entity);
        }

        #region IOrganizationService

        Guid IOrganizationService.Create(Entity entity)
        {
            return CreateRecord(entity);
        }

        OrganizationResponse IOrganizationService.Execute(OrganizationRequest request)
        {
            return ExecuteRequest(request);
        }

        private OrganizationResponse ExecuteRequest(OrganizationRequest request)
        {
            return Service.Execute(request);
        }

        private Guid CreateRecord(Entity entity)
        {
            return Service.Create(entity);
        }

        #endregion

        private IOrganizationService ConnectToCrm()
        {
            return ConnectionProvider.CreateCrmConnection(_connectionString);
        }
    }
}
