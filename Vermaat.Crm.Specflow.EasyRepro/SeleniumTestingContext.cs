using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class SeleniumTestingContext : IDisposable
    {
        private readonly Lazy<Browser> _browser;

        public Browser Browser => _browser.Value;
        public BrowserOptions BrowserOptions { get; private set; }

        public SeleniumTestingContext()
        {
            BrowserOptions = new BrowserOptions()
            {
                CleanSession = true,
                
            };

            _browser = new Lazy<Browser>(() => new Browser(BrowserOptions));
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
