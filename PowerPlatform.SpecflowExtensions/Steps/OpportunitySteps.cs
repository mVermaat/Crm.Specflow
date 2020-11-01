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

        public OpportunitySteps(ICrmContext crmContext, ISeleniumContext selenumContext)
        {
            _crmContext = crmContext;
            _selenumContext = selenumContext;
        }

        [When(@"the opportunity (.*) is closed with the following values")]
        public void WinOpportunity(string alias, Table closeData)
        {
            _crmContext.TableConverter.ConvertTable("opportunityclose", closeData);
            _crmContext.CommandProcessor.Execute(new CloseOpportunityCommand(_crmContext, _selenumContext, alias, closeData));
        }
    }
}
