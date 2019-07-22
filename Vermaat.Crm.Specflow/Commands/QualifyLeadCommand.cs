using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Commands
{
    public class QualifyLeadCommand : ApiOnlyCommandFunc<EntityReferenceCollection>
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
            var lead = new Lead(GlobalTestingContext.ConnectionManager.CurrentConnection.Retrieve(aliasRef, new ColumnSet(Lead.Fields.TransactionCurrencyId, Lead.Fields.CustomerId, Lead.Fields.CampaignId)));

            Logger.WriteLine($"Qualifying Lead {lead.Id}");
            QualifyLeadRequest req = lead.CreateQualifyLeadRequest(_createAccount, _createContact, _createOpportunity);

            return GlobalTestingContext.ConnectionManager.CurrentConnection.Execute<QualifyLeadResponse>(req).CreatedEntities;
        }
    }
}
