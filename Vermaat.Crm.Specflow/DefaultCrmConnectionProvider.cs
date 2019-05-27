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
        public IOrganizationService CreateCrmConnection(CrmConnectionString connection)
        {
            Logger.WriteLine("Connecting to Dynamics CRM API");
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            return new CrmServiceClient(connection.ToCrmClientString());
        }
    }
}
