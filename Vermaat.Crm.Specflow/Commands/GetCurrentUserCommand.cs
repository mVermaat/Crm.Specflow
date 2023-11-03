﻿using Microsoft.Xrm.Sdk;

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
