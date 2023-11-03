using Microsoft.Xrm.Sdk;

namespace Vermaat.Crm.Specflow.Commands
{
    public class GetCurrentUserSettingsCommand : ApiOnlyCommandFunc<EntityReference>
    {
        private readonly string _userAlias;

        public GetCurrentUserSettingsCommand(CrmTestingContext crmContext, string userAlias)
            : base(crmContext)
        {
            _userAlias = userAlias;
        }

        public override EntityReference Execute()
        {
            var entityRef = new EntityReference("usersettings", GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.Id);
            _crmContext.RecordCache.Add(_userAlias, entityRef, false);
            return entityRef;
        }
    }
}
