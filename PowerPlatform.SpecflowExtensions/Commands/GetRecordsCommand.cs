using Microsoft.Xrm.Sdk;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class GetRecordsCommand : ApiOnlyCommandFunc<DataCollection<Entity>>
    {
        private readonly string _entityName;
        private readonly Table _criteria;

        public GetRecordsCommand(ICrmContext crmContext, string entityName, Table criteria) : base(crmContext)
        {
            _entityName = entityName;
            _criteria = criteria;
        }

        public override DataCollection<Entity> Execute()
        {
            Microsoft.Xrm.Sdk.Query.QueryExpression query = HelperMethods.CreateQueryExpressionFromTable(_entityName, _criteria, _crmContext);
            return GlobalContext.ConnectionManager.CurrentConnection.RetrieveMultiple(query).Entities;
        }
    }
}
