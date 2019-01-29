using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace Vermaat.Crm.Specflow.Commands
{
    class UpdateStatusCommand : ApiOnlyCommand
    {
        private readonly string _alias;
        private readonly string _statusCodeText;

        public UpdateStatusCommand(CrmTestingContext crmContext, string alias, string statusCodeText) : base(crmContext)
        {
            _alias = alias;
            _statusCodeText = statusCodeText;
        }

        public override void Execute()
        {
            EntityReference crmRecord = _crmContext.RecordCache[_alias];
            SetStateRequest request = ObjectConverter.ToSetStateRequest(crmRecord, _statusCodeText, _crmContext);
            _crmContext.Service.Execute<SetStateResponse>(request);
        }
    }
}
