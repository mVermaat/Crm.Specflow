using FluentAssertions;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Steps
{
    [Binding]
    public class GeneralSteps
    {
        private readonly ICrmContext _crmContext;

        public GeneralSteps(ICrmContext crmContext)
        {
            _crmContext = crmContext;
        }

    }
}
