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
    public class OpportunitySteps
    {
        private readonly ICrmContext _crmContext;
        private readonly ISeleniumContext _selenumContext;
        private readonly IObjectContainer _container;

        public OpportunitySteps(ICrmContext crmContext, IObjectContainer container)
        {
            _crmContext = crmContext;
            _container = container;
        }

        [When(@"the opportunity (.*) is closed with the following values")]
        public void WinOpportunity(string alias, Table closeData)
        {
            _crmContext.TableConverter.ConvertTable("opportunityclose", closeData);
            _crmContext.CommandProcessor.Execute(new CloseOpportunityCommand(_container, alias, closeData));
        }
    }
}
