using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Query;
using Vermaat.Crm.Specflow.Entities;
using OpenQA.Selenium;
using System.Net;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;

namespace Vermaat.Crm.Specflow.Connectivity
{
    /// <summary>
    /// Connection to Dynamics CRM
    /// </summary>
    public class CrmService : IOrganizationService
    {
        private readonly Lazy<CrmServiceClient> _service;
        private readonly string _connectionString;
        private Lazy<UserSettings> _userSettings;
        private Lazy<Guid> _userId;
        private Guid _callerId;


        private CrmServiceClient Service => _service.Value;
        public UserSettings UserSettings => _userSettings.Value;
        public Guid UserId => _userId.Value;


        public Guid CallerId
        {
            get => _callerId;
            set
            {
                _callerId = value;
                Service.CallerId = CallerId;
                _userSettings = new Lazy<UserSettings>(GetUserSettings);
                _userId = new Lazy<Guid>(GetUserId);
            }
        }

        public CrmService(string connectionString)
        {
            _connectionString = connectionString;
            _service = new Lazy<CrmServiceClient>(ConnectToCrm);
            _userSettings = new Lazy<UserSettings>(GetUserSettings);
            _userId = new Lazy<Guid>(GetUserId);
        }

        public void Create(Entity entity, string alias, AliasedRecordCache recordCache)
        {
            entity.Id = CreateRecord(entity);
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

        public Guid GetUserId()
        {
            if (_callerId != Guid.Empty)
                return _callerId;

            return Execute<WhoAmIResponse>(new WhoAmIRequest()).UserId;
        }

        public UserSettings GetUserSettings()
        {
            var query = new QueryExpression("usersettings")
            {
                TopCount = 1,
                ColumnSet = { AllColumns = true }
            };
            query.Criteria.AddCondition("systemuserid", ConditionOperator.Equal, UserId);
            var settingsEntity = RetrieveMultiple(query).Entities[0];

            query = new QueryExpression("timezonedefinition")
            {
                TopCount = 1
            };
            query.ColumnSet.AddColumn("standardname");
            query.Criteria.AddCondition("timezonecode", ConditionOperator.Equal, settingsEntity["timezonecode"]);
            var timeZoneEntity = RetrieveMultiple(query).Entities[0];
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneEntity.GetAttributeValue<string>("standardname"));

            return new UserSettings(settingsEntity, timeZoneInfo);
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

        internal Guid CreateRecord(Entity entity)
        {
            return Service.Create(entity);
        }

        #endregion

        private CrmServiceClient ConnectToCrm()
        {
            Logger.WriteLine("Connecting to Dynamics CRM API");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new CrmServiceClient(_connectionString);

            if (!client.IsReady)
                throw new TestExecutionException(Constants.ErrorCodes.UNABLE_TO_LOGIN, client.LastCrmException, client.LastCrmError);

            return client;
            
        }
    }
}
