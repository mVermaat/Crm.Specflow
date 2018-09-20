using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow;

namespace Vermaat.Crm.Specflow.Steps
{
    [Binding]
    public class LeadSteps
    {
        private readonly CrmTestingContext _crmContext;
        private readonly StepProcessor _stepProcessor;

        public LeadSteps(CrmTestingContext crmContext, StepProcessor stepProcessor)
        {
            _crmContext = crmContext;
            _stepProcessor = stepProcessor;
        }

        [When(@"(.*) is qualified to a")]
        public void QualifyLead(string alias, Table table)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            var lead = _crmContext.Service.Retrieve(aliasRef, new ColumnSet(Constants.General.CURRENCY, Constants.Lead.CUSTOMER, Constants.Lead.SOURCE_CAMPAIGN));
            var row = table.Rows[0];

            _stepProcessor.LeadProcessor.Qualify(aliasRef, Boolean.Parse(row["Account"]), Boolean.Parse(row["Contact"]), Boolean.Parse(row["Opportunity"]));
        }

    }
}
