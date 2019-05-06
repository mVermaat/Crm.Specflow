using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow.Steps
{
    [Binding]
    public class QuoteSteps
    {
        private readonly CrmTestingContext _crmContext;

        public QuoteSteps(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
        }

        [When(@"the quote (.*) is activated")]
        public void ActivateQuote(string alias)
        {
            _crmContext.CommandProcessor.Execute(new ActivateQuoteCommand(_crmContext, alias));
        }

        [When(@"(.*) is converted to a sales order named (.*)")]
        public void ConvertQuoteToSalesOrder(string quoteAlias, string orderAlias)
        {
            _crmContext.CommandProcessor.Execute(new ConvertToSalesOrderCommand(_crmContext, quoteAlias, orderAlias));
        }

        [When(@"(.*) is activated and converted to a sales order named (.*)")]
        public void ActivateQuoteAndConvertQuoteToSalesOrder(string quoteAlias, string orderAlias)
        {
            _crmContext.CommandProcessor.Execute(new ActivateQuoteCommand(_crmContext, quoteAlias));
            _crmContext.CommandProcessor.Execute(new ConvertToSalesOrderCommand(_crmContext, quoteAlias, orderAlias));
        }
    }
}
