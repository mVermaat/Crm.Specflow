using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Commands
{
    class ConvertToSalesOrderCommand : ApiOnlyCommand
    {
        private readonly string _alias;
        private readonly string _orderAlias;

        public ConvertToSalesOrderCommand(CrmTestingContext crmContext, string quoteAlias, string orderAlias)
            : base(crmContext)
        {
            _alias = quoteAlias;
            _orderAlias = orderAlias;
        }

        public override void Execute()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];
            EntityMetadata metadata = _crmContext.Metadata.GetEntityMetadata(aliasRef.LogicalName);

            Entity record = _crmContext.Service.Retrieve(aliasRef, new ColumnSet(metadata.PrimaryNameAttribute));

            ConvertQuoteToSalesOrderRequest convertSalesOrderRequest = new ConvertQuoteToSalesOrderRequest
            {
                ColumnSet = new ColumnSet(false),
                QuoteId = aliasRef.Id,
                QuoteCloseDate = DateTime.Now,
                QuoteCloseStatus = new OptionSetValue(-1),
                QuoteCloseSubject = record.GetAttributeValue<string>(metadata.PrimaryNameAttribute)
            };
            ConvertQuoteToSalesOrderResponse resp = _crmContext.Service.Execute<ConvertQuoteToSalesOrderResponse>(convertSalesOrderRequest);
            _crmContext.RecordCache.Add(_orderAlias, resp.Entity);
        }
    }
}
