using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using PowerPlatform.SpecflowExtensions.Interfaces;
using PowerPlatform.SpecflowExtensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public abstract class CrmService : ICrmService
    {
        private readonly Lazy<IOrganizationService> _service;

        public abstract Guid UserId { get; set; }
        public abstract UserSettings UserSettings { get; }


        //private readonly Lazy<UserSettings> _userSettings;
        //private readonly Lazy<Guid> _userId;


        protected IOrganizationService Service => _service.Value;

        public CrmService()
        {
            _service = new Lazy<IOrganizationService>(ConnectToCrm);
        }

        //public abstract void Impersonate(Guid userId);
        protected abstract IOrganizationService ConnectToCrm();

        #region CRM Calls 
        public void Create(Entity entity, string alias, AliasedRecordCache recordCache)
        {
            entity.Id = Service.Create(entity);
            recordCache.Add(alias, entity.ToEntityReference());
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
            return (T)Service.Execute(request);
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

        #endregion


        protected UserSettings GetUserSettings(Guid userId)
        {
            var query = new QueryExpression(UserSettings.EntityLogicalName)
            {
                TopCount = 1,
                ColumnSet = { AllColumns = true }
            };
            query.Criteria.AddCondition(UserSettings.Fields.UserId, ConditionOperator.Equal, userId);
            var settingsEntity = RetrieveMultiple(query).Entities[0];

            query = new QueryExpression(TimeZoneDefinition.EntityLogicalName)
            {
                TopCount = 1
            };
            query.ColumnSet.AddColumn(TimeZoneDefinition.Fields.StandardName);
            query.Criteria.AddCondition(TimeZoneDefinition.Fields.TimeZoneCode, ConditionOperator.Equal, settingsEntity[UserSettings.Fields.TimeZoneCode]);
            var timeZoneEntity = RetrieveMultiple(query).Entities[0];
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneEntity.GetAttributeValue<string>(TimeZoneDefinition.Fields.StandardName));

            return new UserSettings(settingsEntity, timeZoneInfo);
        }
        
        
    }
}
