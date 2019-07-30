using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    public class ConvertToSalesOrderCommand : BrowserCommand
    {
        private readonly string _alias;
        private readonly string _orderAlias;

        public ConvertToSalesOrderCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, string quoteAlias, string orderAlias)
            : base(crmContext, seleniumContext)
        {
            _alias = quoteAlias;
            _orderAlias = orderAlias;
        }

        protected override void ExecuteApi()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];
            EntityMetadata metadata = GlobalTestingContext.Metadata.GetEntityMetadata(aliasRef.LogicalName);

            Entity record = GlobalTestingContext.ConnectionManager.CurrentConnection.Retrieve(aliasRef, new ColumnSet(metadata.PrimaryNameAttribute));

            ConvertQuoteToSalesOrderRequest convertSalesOrderRequest = new ConvertQuoteToSalesOrderRequest
            {
                ColumnSet = new ColumnSet(false),
                QuoteId = aliasRef.Id,
                QuoteCloseDate = DateTime.Now,
                QuoteCloseStatus = new OptionSetValue(-1),
                QuoteCloseSubject = record.GetAttributeValue<string>(metadata.PrimaryNameAttribute)
            };
            ConvertQuoteToSalesOrderResponse resp = GlobalTestingContext.ConnectionManager.CurrentConnection.Execute<ConvertQuoteToSalesOrderResponse>(convertSalesOrderRequest);
            _crmContext.RecordCache.Add(_orderAlias, resp.Entity);
        }

        protected override void ExecuteBrowser()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];

            FormData formData = _seleniumContext.GetBrowser().OpenRecord(new OpenFormOptions(aliasRef));
            EntityReference order = formData.CommandBar.CreateOrder();
            _crmContext.RecordCache.Add(_orderAlias, order);
        }
    }
}
