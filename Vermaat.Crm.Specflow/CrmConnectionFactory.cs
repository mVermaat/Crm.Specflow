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
    internal static class CrmConnectionFactory
    {
        public static CrmService CreateCrmConnection(CrmConnectionString connection)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var client = new CrmServiceClient(connection.ToCrmClientString());
            return new CrmService(client);
        }
    }
}
