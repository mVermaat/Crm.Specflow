using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Vermaat.Crm.Specflow.EasyRepro;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Commands
{
    public class ActivateQuoteCommand : BrowserCommand
    {
        private readonly string _alias;

        public ActivateQuoteCommand(CrmTestingContext crmContext, SeleniumTestingContext selenumContext, string alias) 
            : base(crmContext, selenumContext)
        {
            _alias = alias;
        }

        protected override void ExecuteApi()
        {
            var aliasRef = _crmContext.RecordCache[_alias];

            var activateQuote = new SetStateRequest()
            {
                EntityMoniker = aliasRef,
                State = new OptionSetValue(1),//Active
                Status = new OptionSetValue(2)//In progress
            };

            GlobalTestingContext.ConnectionManager.CurrentConnection.Execute<SetStateResponse>(activateQuote);
        }

        protected override void ExecuteBrowser()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];

            FormData formData = _seleniumContext.GetBrowser().OpenRecord(new OpenFormOptions(aliasRef));
            formData.CommandBar.ActivateQuote();
        }
    }
}
