using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Commands
{
    public class GetCurrentUserCommand : ApiOnlyCommandFunc<EntityReference>
    {
        private readonly string _userAlias;

        public GetCurrentUserCommand(CrmTestingContext crmContext, string userAlias) 
            : base(crmContext)
        {
            _userAlias = userAlias;
        }

        public override EntityReference Execute()
        {
            var entityRef = new EntityReference("systemuser", GlobalTestingContext.ConnectionManager.CurrentConnection.UserId);
            _crmContext.RecordCache.Add(_userAlias, entityRef, false);
            return entityRef;
        }
    }
}
