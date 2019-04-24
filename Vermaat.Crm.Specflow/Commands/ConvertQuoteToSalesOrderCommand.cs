using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;

namespace Vermaat.Crm.Specflow.Commands
{
    public class ConverQuoteToSalesOrderCommand : ApiOnlyCommandFunc<Entity>
    {
        private readonly string _alias;

        public ConverQuoteToSalesOrderCommand(CrmTestingContext crmContext, string alias) : base(crmContext)
        {
            _alias = alias;
        }

        public override Entity Execute()
        {
            var aliasRef = _crmContext.RecordCache[_alias];

            var activateQuote = new ConvertQuoteToSalesOrderRequest()
            {
                QuoteId = aliasRef.Id,
                ColumnSet = new ColumnSet(true),
            };

            return _crmContext.Service.Execute<ConvertQuoteToSalesOrderResponse>(activateQuote).Entity;
        }
    }
}
