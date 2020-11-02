using BoDi;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.EasyRepro;
using PowerPlatform.SpecflowExtensions.EasyRepro.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class ReviseQuoteCommand : BrowserCommand
    {
        private readonly string _toReviseAlias;
        private readonly string _newQuoteAlias;

        public ReviseQuoteCommand(IObjectContainer container, string toReviseAlias, string newQuoteAlias)
            : base(container)
        {
            _toReviseAlias = toReviseAlias;
            _newQuoteAlias = newQuoteAlias;
        }

        protected override void ExecuteApi()
        {
            var aliasRef = _crmContext.RecordCache[_toReviseAlias];
            var metadata = GlobalContext.Metadata.GetEntityMetadata(aliasRef.LogicalName);

            var quoteClose = new Entity("quoteclose");
            quoteClose["quoteid"] = aliasRef;
            quoteClose["subject"] = "Closed to revise";


            GlobalContext.ConnectionManager.CurrentCrmService.Execute<CloseQuoteResponse>(new CloseQuoteRequest()
            {
                Status = new OptionSetValue(-1),
                QuoteClose = quoteClose
            });

            var response = GlobalContext.ConnectionManager.CurrentCrmService.Execute<ReviseQuoteResponse>(new ReviseQuoteRequest()
            {
                ColumnSet = new ColumnSet(metadata.PrimaryNameAttribute),
                QuoteId = aliasRef.Id
            });
            _crmContext.RecordCache.Add(_newQuoteAlias, response.Entity);
        }

        protected override void ExecuteBrowser()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_toReviseAlias];

            var browser = GlobalContext.ConnectionManager.GetCurrentBrowserSession(_seleniumContext);
            var ceApp = browser.GetApp<CustomerEngagementApp>(_container);
            ceApp.OpenRecord(new OpenFormOptions(aliasRef));
            var revisedQuote = browser.GetApp<QuoteActions>(_container).ReviseQuote(ceApp.Navigation);
            _crmContext.RecordCache.Add(_newQuoteAlias, revisedQuote);
        }
    }
}
