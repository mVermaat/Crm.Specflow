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
        private IOrganizationService _service;

        public AliasedRecordCache RecordCache { get; set; }

        public CrmService(IOrganizationService service)
        {
            _service = service;
        }

        public void Create(Entity entity, string alias)
        {
            entity.Id = CreateRecord(entity);
            RecordCache.Add(alias, entity.ToEntityReference());
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            _service.Associate(entityName, entityId, relationship, relatedEntities);
        }

        public void Delete(string entityName, Guid id)
        {
            _service.Delete(entityName, id);
        }

        public void Delete(EntityReference entityReference)
        {
            _service.Delete(entityReference.LogicalName, entityReference.Id);
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            _service.Disassociate(entityName, entityId, relationship, relatedEntities);
        }

        public T Execute<T>(OrganizationRequest request) where T : OrganizationResponse
        {
            return (T)ExecuteRequest(request);
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            return _service.Retrieve(entityName, id, columnSet);
        }

        public Entity Retrieve(EntityReference entityReference, ColumnSet columnSet)
        {
            return _service.Retrieve(entityReference.LogicalName, entityReference.Id, columnSet);
        }

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            return _service.RetrieveMultiple(query);
        }

        public void Update(Entity entity)
        {
            _service.Update(entity);
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
            return _service.Execute(request);
        }

        private Guid CreateRecord(Entity entity)
        {
            return _service.Create(entity);
        }

        #endregion
    }
}
