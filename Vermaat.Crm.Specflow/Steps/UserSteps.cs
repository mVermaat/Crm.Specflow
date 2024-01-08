using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow.Steps
{
    [Binding]
    public class UserSteps
    {
        private readonly CrmTestingContext _crmContext;
        private readonly SeleniumTestingContext _selenumContext;
        private readonly UserProfileHandler _userProfileHandler;

        public UserSteps(CrmTestingContext crmContext, SeleniumTestingContext selenumContext, UserProfileHandler userProfileHandler)
        {
            _crmContext = crmContext;
            _selenumContext = selenumContext;
            _userProfileHandler = userProfileHandler;
        }

        [Given(@"a logged in '(.*)'")]
        public void LoginWithUser(string profile)
        {
            _crmContext.CommandProcessor.Execute(new LoginWithUserCommand(_crmContext, _userProfileHandler.GetProfile(profile)));
        }

        [Given(@"a logged in '(.*)' named ([^\s]+)")]
        public void LoginWithUser(string profile, string alias)
        {
            LoginWithUser(profile);
            _crmContext.CommandProcessor.Execute(new GetCurrentUserCommand(_crmContext, alias));
        }

        [Given(@"the current logged in user named (.*)")]
        public void GetLoggedInUser(string alias)
        {
            _crmContext.CommandProcessor.Execute(new GetCurrentUserCommand(_crmContext, alias));
        }


        [Given(@"the current logged in user's settings named (.*)")]
        public void GetLoggedInUserSettings(string alias)
        {
            _crmContext.CommandProcessor.Execute(new GetCurrentUserSettingsCommand(_crmContext, alias));
        }

    }
}
