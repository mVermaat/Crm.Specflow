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

        public WinQuoteCommand(CrmTestingContext crmContext, string alias) : base(crmContext)
        {
            _alias = alias;
        }

        public override WinQuoteResponse Execute()
        {
            var aliasRef = _crmContext.RecordCache[_alias];

            var quoteClose = new Entity("quoteclose");
            quoteClose.Attributes.Add("subject", "Quote Close " + DateTime.Now.ToString());
            quoteClose.Attributes.Add("quoteid", aliasRef);

            var winQuoteRequest = new WinQuoteRequest()
            {
                QuoteClose = quoteClose,
                Status = new OptionSetValue(4)
            }; 

            return _crmContext.Service.Execute<WinQuoteResponse>(winQuoteRequest);
        }
    }
}
