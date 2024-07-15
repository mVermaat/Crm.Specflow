using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Net;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Connectivity
{
    /// <summary>
    /// Connection to Dynamics CRM
    /// </summary>
    public class CrmService : CrmServiceBase
    {
        private CrmServiceClient _client;

        public CrmService(string connectionString) : base(connectionString)
        {
        }

        protected override IOrganizationService ConnectToCrm()
        {
            Logger.WriteLine("Connecting to Dynamics CRM API");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _client = new CrmServiceClient(ConnectionString);

            if (!_client.IsReady)
                throw new TestExecutionException(Constants.ErrorCodes.UNABLE_TO_LOGIN, _client.LastCrmException, _client.LastCrmError);

            return _client;

        }

        protected override void Impersonate(Guid value)
        {
            ((CrmServiceClient)Service).CallerId = value;
        }
    }
}
