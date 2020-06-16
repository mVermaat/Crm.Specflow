using BoDi;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Hooks
{

    [Binding]
    public class DependencyInjectionRegistration
    {
        private readonly IObjectContainer _objectContainer;

        public DependencyInjectionRegistration(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void RegisterInterfaces()
        {
            _objectContainer.RegisterTypeAs<CrmContext, ICrmContext>();
        }
    }
}
