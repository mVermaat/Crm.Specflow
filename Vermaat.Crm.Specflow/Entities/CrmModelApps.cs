using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Entities
{
    public class CrmModelApps
    {
        private Dictionary<string, Entity> _apps;

        private CrmModelApps(IEnumerable<Entity> apps)
        {
            _apps = apps.ToDictionary(a => a.GetAttributeValue<string>("uniquename"));
        }

        public Guid GetAppId(string uniqueAppName)
        {
            if (!_apps.ContainsKey(uniqueAppName))
                throw new TestExecutionException(Constants.ErrorCodes.APP_NOT_FOUND, uniqueAppName);

            return _apps[uniqueAppName].Id;
        }

        public static CrmModelApps GetApps(CrmService service)
        {
            var query = new QueryExpression("appmodule");
            query.ColumnSet.AddColumns("uniquename");

            var result = service.RetrieveMultiple(query);

            return new CrmModelApps(result.Entities);
        }
    }
}
