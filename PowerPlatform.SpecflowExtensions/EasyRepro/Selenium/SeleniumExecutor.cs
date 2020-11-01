using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    internal class SeleniumExecutor : ISeleniumExecutor
    {
        private static readonly SeleniumSelectorData _selectors = new SeleniumSelectorData();

        private readonly BrowserPage _browserPage;
        private readonly XrmApp _xrmApp;

        public SeleniumExecutor(BrowserPage browserPage, XrmApp xrmApp)
        {
            _browserPage = browserPage;
            _xrmApp = xrmApp;
        }

        public TResult Execute<TResult>(string commmandName, Func<IWebDriver, TResult> func)
        {
            return _browserPage.Execute(GetOptions(commmandName), func).Value;
        }

        public TResult Execute<TResult>(string commmandName, Func<IWebDriver, SeleniumSelectorData, TResult> func)
        {
            return _browserPage.Execute(GetOptions(commmandName), func, _selectors).Value;
        }

        public TResult Execute<TResult>(string commmandName, Func<IWebDriver, SeleniumSelectorData, XrmApp, TResult> func)
        {
            return _browserPage.Execute(GetOptions(commmandName), func, _selectors, _xrmApp).Value;
        }

        private BrowserCommandOptions GetOptions(string commandName)
        {
            return new BrowserCommandOptions(
                commandName: commandName,
                retryAttempts: 0,
                throwExceptions: true,
                exceptions: new Type[] { typeof(NoSuchElementException), typeof(StaleElementReferenceException) });
        }
    }
}
