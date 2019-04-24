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

        [When(@"a quote named (.*) is activated")]
        public void ActivateQuote(string alias)
        {
            _crmContext.CommandProcessor.Execute(new ActivateQuoteCommand(_crmContext, alias));
        }

        [When(@"a quote named (.*) is converted to a sales order")]
        public void ConverQuoteToSalesOrder(string alias)
        {
            _crmContext.CommandProcessor.Execute(new ConverQuoteToSalesOrderCommand(_crmContext, alias));
        }

        [When(@"a quote named (.*) is won")]
        public void WinQuote(string alias)
        {
            _crmContext.CommandProcessor.Execute(new WinQuoteCommand(_crmContext, alias));
        }
    }
}
