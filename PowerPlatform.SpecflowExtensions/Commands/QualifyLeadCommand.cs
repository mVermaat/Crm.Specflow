using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.Interfaces;
using PowerPlatform.SpecflowExtensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class QualifyLeadCommand : ApiOnlyCommandFunc<EntityReferenceCollection>
    {
        private readonly string _alias;
        private readonly bool _createAccount;
        private readonly bool _createContact;
        private readonly bool _createOpportunity;

        public QualifyLeadCommand(ICrmContext crmContext, string alias, bool createAccount, bool createContact, bool createOpportunity) : base(crmContext)
        {
            _alias = alias;
            _createAccount = createAccount;
            _createContact = createContact;
            _createOpportunity = createOpportunity;
        }

        public override EntityReferenceCollection Execute()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];
            var lead = GlobalContext.ConnectionManager.CurrentConnection.Retrieve(aliasRef, 
                new ColumnSet(Lead.Fields.TransactionCurrencyId, Lead.Fields.CustomerId, Lead.Fields.CampaignId));

            Logger.WriteLine($"Qualifying Lead {lead.Id}");
            QualifyLeadRequest req = CreateQualifyLeadRequest(lead, _createAccount, _createContact, _createOpportunity);

            return GlobalContext.ConnectionManager.CurrentConnection.Execute<QualifyLeadResponse>(req).CreatedEntities;
        }

        private QualifyLeadRequest CreateQualifyLeadRequest(Entity lead, bool createAccount, bool createContact, bool createOpportunity)
        {
            var req = new QualifyLeadRequest()
            {
                CreateAccount = createAccount,
                CreateContact = createContact,
                CreateOpportunity = createOpportunity,
                LeadId = lead.ToEntityReference(),
                OpportunityCurrencyId = lead.GetAttributeValue<EntityReference>(Lead.Fields.TransactionCurrencyId),
                OpportunityCustomerId = lead.GetAttributeValue<EntityReference>(Lead.Fields.CustomerId),
                SourceCampaignId = lead.GetAttributeValue<EntityReference>(Lead.Fields.CampaignId),
                Status = new OptionSetValue((int)Lead_StatusCode.Qualified)
            };
            req.Parameters.Add("SuppressDuplicateDetection", true);
            return req;
        }
    }
}
