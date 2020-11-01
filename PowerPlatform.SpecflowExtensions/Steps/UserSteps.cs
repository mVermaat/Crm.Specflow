using BoDi;
using PowerPlatform.SpecflowExtensions.Commands;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Steps
{
    [Binding]
    public class UserSteps
    {
        private readonly ICrmContext _crmContext;
        private readonly IObjectContainer _container;

        public UserSteps(ICrmContext crmContext, IObjectContainer container)
        {
            _crmContext = crmContext;
            _container = container;
        }

        // TODO: Check if tested
        //[Given(@"the current logged in user named (.*)")]
        //public void GetLoggedInUser(string alias)
        //{
        //    _crmContext.CommandProcessor.Execute(new GetCurrentUserCommand(_crmContext, alias));
        //}

        [Given(@"the current logged in user's settings named (.*)")]
        public void GetLoggedInUserSettings(string alias)
        {
            _crmContext.CommandProcessor.Execute(new GetCurrentUserSettingsCommand(_container, alias));
        }

    }
}
