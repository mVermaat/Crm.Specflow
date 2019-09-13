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
    public class OpportunitySteps
    {
        private readonly CrmTestingContext _crmContext;
        private readonly SeleniumTestingContext _selenumContext;

        public OpportunitySteps(CrmTestingContext crmContext, SeleniumTestingContext selenumContext)
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
