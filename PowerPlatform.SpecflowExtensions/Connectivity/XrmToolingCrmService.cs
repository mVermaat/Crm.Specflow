using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public class XrmToolingCrmService : CrmService
    {
        private readonly string _connectionString;

        public override Guid CallerId 
        { 
            get => ((CrmServiceClient)Service).CallerId;
            set => ((CrmServiceClient)Service).CallerId = value;
        }

        public XrmToolingCrmService(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override IOrganizationService ConnectToCrm()
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
