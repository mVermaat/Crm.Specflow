using Microsoft.Xrm.Sdk;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class GetCurrentUserSettingsCommand : ApiOnlyCommandFunc<EntityReference>
    {
        private readonly string _userAlias;

        public GetCurrentUserSettingsCommand(ICrmContext crmContext, string userAlias)
            : base(crmContext)
        {
            _userAlias = userAlias;
        }

        public override EntityReference Execute()
        {
            var entityRef = new EntityReference("usersettings", GlobalContext.ConnectionManager.CurrentConnection.UserSettings.Id);
            _crmContext.RecordCache.Add(_userAlias, entityRef, false);
            return entityRef;
        }
    }
}
