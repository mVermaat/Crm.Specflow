using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;
using Vermaat.Crm.Specflow.EasyRepro.UCI;
using Vermaat.Crm.Specflow.EasyRepro.Web;

namespace Vermaat.Crm.Specflow
{
    public class SeleniumTestingContext : IDisposable
    {
        private readonly Lazy<IBrowser> _browser;
        private readonly bool _uci;

        public IBrowser Browser => _browser.Value;
        public BrowserOptions BrowserOptions { get; }
        public ButtonTexts ButtonTexts { get; set; }

        public SeleniumTestingContext()
        {
            _uci = bool.Parse(HelperMethods.GetAppSettingsValue("UCI"));
            ButtonTexts = new ButtonTexts();
            BrowserOptions = new BrowserOptions()
            {
                CleanSession = true,
            };

            _browser = new Lazy<IBrowser>(() => _uci ? new UCIBrowser(BrowserOptions, ButtonTexts) as IBrowser : new WebBrowser(BrowserOptions, ButtonTexts));
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
