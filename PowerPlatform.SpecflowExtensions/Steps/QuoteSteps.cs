using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Steps
{
    [Binding]
    public class QuoteSteps
    {
        private readonly ICrmContext _crmContext;
        private readonly ISeleniumContext _selenumContext;

        public QuoteSteps(ICrmContext crmContext, ISeleniumContext selenumContext)
        {
            _crmContext = crmContext;
            _selenumContext = selenumContext;
        }

        //[When(@"the quote (.*) is activated")]
        //public void ActivateQuote(string alias)
        //{
        //    _crmContext.CommandProcessor.Execute(new ActivateQuoteCommand(_crmContext, _selenumContext, alias));
        //}

        //[When(@"(.*) is converted to a sales order named (.*)")]
        //public void ConvertQuoteToSalesOrder(string quoteAlias, string orderAlias)
        //{
        //    _crmContext.CommandProcessor.Execute(new ConvertToSalesOrderCommand(_crmContext, _selenumContext, quoteAlias, orderAlias));
        //}

        //[When(@"(.*) is activated and converted to a sales order named (.*)")]
        //public void ActivateQuoteAndConvertQuoteToSalesOrder(string quoteAlias, string orderAlias)
        //{
        //    _crmContext.CommandProcessor.Execute(new ActivateQuoteCommand(_crmContext, _selenumContext, quoteAlias));
        //    _crmContext.CommandProcessor.Execute(new ConvertToSalesOrderCommand(_crmContext, _selenumContext, quoteAlias, orderAlias));
        //}

        //[When(@"(.*) is revised and its revised quote is named (.*)")]
        //public void ReviseQuote(string quoteAlias, string newQuoteAlias)
        //{
        //    _crmContext.CommandProcessor.Execute(new ReviseQuoteCommand(_crmContext, _selenumContext, quoteAlias, newQuoteAlias));
        //}
    }
}
