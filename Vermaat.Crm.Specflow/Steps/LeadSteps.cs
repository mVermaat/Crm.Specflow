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
        private CrmTestingContext _crmContext;

        public LeadSteps(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
        }

        [When(@"(.*) is qualified to a")]
        public void QualifyLead(string alias, Table table)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            var lead = _crmContext.Service.Retrieve(aliasRef, new ColumnSet(Constants.General.CURRENCY, Constants.Lead.CUSTOMER, Constants.Lead.SOURCE_CAMPAIGN));

            var row = table.Rows[0];

            QualifyLeadRequest req = new QualifyLeadRequest()
            {
                CreateAccount = Boolean.Parse(row["Account"]),
                CreateContact = Boolean.Parse(row["Contact"]),
                CreateOpportunity = Boolean.Parse(row["Opportunity"]),
                LeadId = aliasRef,
                OpportunityCurrencyId = lead.GetAttributeValue<EntityReference>(Constants.General.CURRENCY),
                OpportunityCustomerId = lead.GetAttributeValue<EntityReference>(Constants.Lead.CUSTOMER),
                SourceCampaignId = lead.GetAttributeValue<EntityReference>(Constants.Lead.SOURCE_CAMPAIGN),
                Status = new OptionSetValue(Constants.Lead.QUALIFIED_VALUE)
            };

            var resp = _crmContext.Service.Execute<QualifyLeadResponse>(req);

            foreach(var record in resp.CreatedEntities)
            {
                _crmContext.RecordCache.Add(string.Format("{0}_{1}", alias, record.LogicalName), record);
            }
        }

    }
}
