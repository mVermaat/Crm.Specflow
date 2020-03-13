using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow.Steps
{
    [Binding]
    public class UserSteps
    {
        private readonly CrmTestingContext _crmContext;
        private readonly SeleniumTestingContext _selenumContext;

        public UserSteps(CrmTestingContext crmContext, SeleniumTestingContext selenumContext)
        {
            _crmContext = crmContext;
            _selenumContext = selenumContext;
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
