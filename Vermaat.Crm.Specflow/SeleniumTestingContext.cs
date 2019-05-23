using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow
{
    public class SeleniumTestingContext : IDisposable
    {
        private readonly Lazy<UCIBrowser> _browser;
        private readonly CrmTestingContext _crmContext;
        private readonly string[] _targets;

        public UCIBrowser Browser => _browser.Value;
        public BrowserOptions BrowserOptions { get; }
        public ButtonTexts ButtonTexts { get; set; }

        public SeleniumTestingContext(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
            ButtonTexts = new ButtonTexts();
            BrowserOptions = new BrowserOptions()
            {
                CleanSession = true,
            };

            _browser = new Lazy<UCIBrowser>(InitializeBrowser);

            _targets = ConfigurationManager.AppSettings["Target"]
                .ToLower()
                .Split(';')
                .Select(splitted => splitted.Trim())
                .ToArray();
        }

        public bool IsTarget(string target)
        {
            if (string.IsNullOrWhiteSpace(target))
                return false;

            return _targets.Contains(target.ToLower());
        }

        public void Dispose()
        {
            if (_browser.IsValueCreated)
            {
                _browser.Value.Dispose();
            }
        }

        private UCIBrowser InitializeBrowser()
        {
            var browser = new UCIBrowser(BrowserOptions, ButtonTexts);
            browser.Login(_crmContext.ConnectionInfo);
            return browser;
        }
    }
}
