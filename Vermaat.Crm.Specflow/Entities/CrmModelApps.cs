﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vermaat.Crm.Specflow.Entities
{
    public class CrmModelApps
    {
        private readonly Dictionary<string, Entity> _apps;

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

        public static CrmModelApps GetApps()
        {
            var query = new QueryExpression("appmodule");
            query.ColumnSet.AddColumns("uniquename");

            var result = GlobalTestingContext.ConnectionManager.AdminConnection.RetrieveMultiple(query);

            return new CrmModelApps(result.Entities);
        }
    }
}
