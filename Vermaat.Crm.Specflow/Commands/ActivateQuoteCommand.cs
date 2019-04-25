using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Commands
{
    public class ActivateQuoteCommand : ApiOnlyCommandFunc<SetStateResponse>
    {
        private readonly string _alias;

        public ActivateQuoteCommand(CrmTestingContext crmContext, string alias) : base(crmContext)
        {
            _alias = alias;
        }

        public override SetStateResponse Execute()
        {
            var aliasRef = _crmContext.RecordCache[_alias];

            var activateQuote = new SetStateRequest()
            {
                EntityMoniker = aliasRef,
                State = new OptionSetValue(1),//Active
                Status = new OptionSetValue(2)//In progress
            };

            return _crmContext.Service.Execute<SetStateResponse>(activateQuote);
        }
    }
}
