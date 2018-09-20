using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Processors
{
    public class LeadProcessor : ILeadProcessor
    {
        private readonly CrmTestingContext _crmContext;

        public LeadProcessor(CrmTestingContext context)
        {
            _crmContext = context;
        }

        public EntityReferenceCollection Qualify(EntityReference leadReference, bool createAccount, bool createContact, bool createOpportunity)
        {
            var lead = _crmContext.Service.Retrieve(leadReference, new ColumnSet(Constants.General.CURRENCY, Constants.Lead.CUSTOMER, Constants.Lead.SOURCE_CAMPAIGN));

            QualifyLeadRequest req = new QualifyLeadRequest()
            {
                CreateAccount = createAccount,
                CreateContact = createContact,
                CreateOpportunity = createOpportunity,
                LeadId = leadReference,
                OpportunityCurrencyId = lead.GetAttributeValue<EntityReference>(Constants.General.CURRENCY),
                OpportunityCustomerId = lead.GetAttributeValue<EntityReference>(Constants.Lead.CUSTOMER),
                SourceCampaignId = lead.GetAttributeValue<EntityReference>(Constants.Lead.SOURCE_CAMPAIGN),
                Status = new OptionSetValue(Constants.Lead.QUALIFIED_VALUE)
            };

            return _crmContext.Service.Execute<QualifyLeadResponse>(req).CreatedEntities;

        }
    }
}
