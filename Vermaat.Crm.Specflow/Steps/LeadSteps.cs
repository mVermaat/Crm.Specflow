using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow.Steps
{
    [Binding]
    public class LeadSteps
    {
        private readonly CrmTestingContext _crmContext;

        public LeadSteps(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
        }

        [When(@"(.*) is qualified to a")]
        public void QualifyLead(string alias, Table table)
        {
            TableRow row = table.Rows[0];
            _crmContext.CommandProcessor.Execute(new QualifyLeadCommand(_crmContext, alias, 
                bool.Parse(row["Account"]), bool.Parse(row["Contact"]), bool.Parse(row["Opportunity"])));
        }
    }
}
