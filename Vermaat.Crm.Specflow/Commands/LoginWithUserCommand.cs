using Vermaat.Crm.Specflow.Connectivity;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Commands
{
    public class LoginWithUserCommand : ApiOnlyCommand
    {
        private readonly UserProfile _userProfile;

        public LoginWithUserCommand(CrmTestingContext crmContext, UserProfile userProfile)
            : base(crmContext)
        {
            _userProfile = userProfile;
        }

        public override void Execute()
        {
            GlobalTestingContext.ConnectionManager.SetCurrentConnection(CrmConnectionFactory.CreateNewConnection(_userProfile));
            Logger.WriteLine($"Successfully logged in with {_userProfile.Profile}");
        }




    }
}
