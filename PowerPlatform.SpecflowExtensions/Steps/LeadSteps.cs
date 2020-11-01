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
    public class LeadSteps
    {
        private readonly ICrmContext _crmContext;
        private readonly IObjectContainer _container;

        public LeadSteps(ICrmContext crmContext, IObjectContainer container)
        {
            _crmContext = crmContext;
            _container = container;
        }

        [When(@"(.*) is qualified to a")]
        public void QualifyLead(string alias, Table table)
        {
            TableRow row = table.Rows[0];
            _crmContext.CommandProcessor.Execute(new QualifyLeadCommand(_container, alias,
                bool.Parse(row["Account"]), bool.Parse(row["Contact"]), bool.Parse(row["Opportunity"])));
        }
    }
}
