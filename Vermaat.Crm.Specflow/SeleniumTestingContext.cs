using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow
{
    public class SeleniumTestingContext : IDisposable
    {
        private readonly Lazy<UCIBrowser> _browser;

        public UCIBrowser Browser => _browser.Value;
        public BrowserOptions BrowserOptions { get; }
        public ButtonTexts ButtonTexts { get; set; }

        public SeleniumTestingContext()
        {
            ButtonTexts = new ButtonTexts();
            BrowserOptions = new BrowserOptions()
            {
                CleanSession = true,
            };

            _browser = new Lazy<UCIBrowser>(() => new UCIBrowser(BrowserOptions, ButtonTexts));
        }

        public void Dispose()
        {
            if (_browser.IsValueCreated)
            {
                _browser.Value.Dispose();
            }
        }
    }
}
