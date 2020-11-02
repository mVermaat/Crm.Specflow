using BoDi;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
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
    public class ConvertToSalesOrderCommand : BrowserCommand
    {
        private readonly string _alias;
        private readonly string _orderAlias;

        public ConvertToSalesOrderCommand(IObjectContainer container, string quoteAlias, string orderAlias)
            : base(container)
        {
            _alias = quoteAlias;
            _orderAlias = orderAlias;
        }

        protected override void ExecuteApi()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];
            var metadata = GlobalContext.Metadata.GetEntityMetadata(aliasRef.LogicalName);

            Entity record = GlobalContext.ConnectionManager.CurrentCrmService.Retrieve(aliasRef, new ColumnSet(metadata.PrimaryNameAttribute));

            ConvertQuoteToSalesOrderRequest convertSalesOrderRequest = new ConvertQuoteToSalesOrderRequest
            {
                ColumnSet = new ColumnSet(false),
                QuoteId = aliasRef.Id,
                QuoteCloseDate = DateTime.Now,
                QuoteCloseStatus = new OptionSetValue(-1),
                QuoteCloseSubject = record.GetAttributeValue<string>(metadata.PrimaryNameAttribute)
            };
            ConvertQuoteToSalesOrderResponse resp = GlobalContext.ConnectionManager.CurrentCrmService.Execute<ConvertQuoteToSalesOrderResponse>(convertSalesOrderRequest);
            _crmContext.RecordCache.Add(_orderAlias, resp.Entity);
        }

        protected override void ExecuteBrowser()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];

            var browser = GlobalContext.ConnectionManager.GetCurrentBrowserSession(_seleniumContext);
            var ceApp = browser.GetApp<CustomerEngagementApp>(_container);
            ceApp.OpenRecord(new OpenFormOptions(aliasRef));
            
            var order = browser.GetApp<QuoteActions>(_container).CreateOrder(ceApp.Navigation);
            _crmContext.RecordCache.Add(_orderAlias, order);
        }
    }
}
