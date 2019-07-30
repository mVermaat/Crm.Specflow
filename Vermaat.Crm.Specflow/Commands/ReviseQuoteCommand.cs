using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    public class ReviseQuoteCommand : BrowserCommand
    {
        private readonly string _toReviseAlias;
        private readonly string _newQuoteAlias;

        public ReviseQuoteCommand(CrmTestingContext crmContext, SeleniumTestingContext selenumContext, string toReviseAlias, string newQuoteAlias)
            : base(crmContext, selenumContext)
        {
            _toReviseAlias = toReviseAlias;
            _newQuoteAlias = newQuoteAlias;
        }

        protected override void ExecuteApi()
        {
            var aliasRef = _crmContext.RecordCache[_toReviseAlias];
            EntityMetadata metadata = GlobalTestingContext.Metadata.GetEntityMetadata(aliasRef.LogicalName);

            var quoteClose = new Entity("quoteclose");
            quoteClose["quoteid"] = aliasRef;
            quoteClose["subject"] = "Closed to revise";


            GlobalTestingContext.ConnectionManager.CurrentConnection.Execute<CloseQuoteResponse>(new CloseQuoteRequest()
            {
                Status = new OptionSetValue(-1),
                QuoteClose = quoteClose                
            });

            var response = GlobalTestingContext.ConnectionManager.CurrentConnection.Execute<ReviseQuoteResponse>(new ReviseQuoteRequest()
            {
                ColumnSet = new ColumnSet(metadata.PrimaryNameAttribute),
                QuoteId = aliasRef.Id
            });
            _crmContext.RecordCache.Add(_newQuoteAlias, response.Entity);
        }

        protected override void ExecuteBrowser()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_toReviseAlias];

            FormData formData = _seleniumContext.GetBrowser().OpenRecord(new OpenFormOptions(aliasRef));
            var revisedQuote = formData.CommandBar.ReviseQuote();
            _crmContext.RecordCache.Add(_newQuoteAlias, revisedQuote);
        }
    }
}
