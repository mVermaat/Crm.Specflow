using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Commands
{
    public class WinQuoteCommand : ApiOnlyCommandFunc<WinQuoteResponse>
    {
        private readonly string _alias;
        private readonly string _subject;

        public WinQuoteCommand(CrmTestingContext crmContext, string alias, string subject) : base(crmContext)
        {
            _alias = alias;
            _subject = subject;
        }

        public override WinQuoteResponse Execute()
        {
            var aliasRef = _crmContext.RecordCache[_alias];

            var quoteClose = new Entity("quoteclose");
            quoteClose.Attributes.Add("subject", _subject);
            quoteClose.Attributes.Add("quoteid", aliasRef);

            var winQuoteRequest = new WinQuoteRequest()
            {
                QuoteClose = quoteClose,
                Status = new OptionSetValue(-1)
            };

            return _crmContext.Service.Execute<WinQuoteResponse>(winQuoteRequest);
        }
    }
}
