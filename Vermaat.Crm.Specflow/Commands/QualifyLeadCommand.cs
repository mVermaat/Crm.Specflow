using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Commands
{
    class QualifyLeadCommand : ApiOnlyCommandFunc<EntityReferenceCollection>
    {
        private readonly string _alias;
        private readonly bool _createAccount;
        private readonly bool _createContact;
        private readonly bool _createOpportunity;

        public QualifyLeadCommand(CrmTestingContext crmContext, string alias, bool createAccount, bool createContact, bool createOpportunity) : base(crmContext)
        {
            _alias = alias;
            _createAccount = createAccount;
            _createContact = createContact;
            _createOpportunity = createOpportunity;
        }

        public override EntityReferenceCollection Execute()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];
            Entity lead = _crmContext.Service.Retrieve(aliasRef, new ColumnSet(Lead.Fields.TransactionCurrencyId, Lead.Fields.CustomerId, Lead.Fields.CampaignId));

            QualifyLeadRequest req = new QualifyLeadRequest()
            {
                CreateAccount = _createAccount,
                CreateContact = _createContact,
                CreateOpportunity = _createOpportunity,
                LeadId = aliasRef,
                OpportunityCurrencyId = lead.GetAttributeValue<EntityReference>(Lead.Fields.TransactionCurrencyId),
                OpportunityCustomerId = lead.GetAttributeValue<EntityReference>(Lead.Fields.CustomerId),
                SourceCampaignId = lead.GetAttributeValue<EntityReference>(Lead.Fields.CampaignId),
                Status = new OptionSetValue((int)Lead_StatusCode.Qualified)
            };
            req.Parameters.Add("SuppressDuplicateDetection", true);

            return _crmContext.Service.Execute<QualifyLeadResponse>(req).CreatedEntities;
        }
    }
}
