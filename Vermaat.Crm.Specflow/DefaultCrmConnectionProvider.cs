using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    internal class DefaultCrmConnectionProvider : ICrmConnectionProvider
    {
        public IOrganizationService CreateCrmConnection(string connectionString)
        {
            Logger.WriteLine("Connecting to Dynamics CRM API");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return new CrmServiceClient(connectionString);
        }
    }
}
