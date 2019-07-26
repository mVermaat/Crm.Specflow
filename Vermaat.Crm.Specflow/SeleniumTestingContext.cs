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
    public class SeleniumTestingContext
    {

        private readonly CrmTestingContext _crmContext;

        public BrowserOptions BrowserOptions { get; }
        public string CurrentApp { get; set; }

        public SeleniumTestingContext(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
            BrowserOptions = new BrowserOptions()
            {
                CleanSession = true,
            };
            CurrentApp = HelperMethods.GetAppSettingsValue("AppName", true);

        }

        public UCIBrowser GetBrowser()
        {
            if (_crmContext.IsTarget("API"))
                throw new InvalidOperationException("Cannot start the browser if the target is API");

            var browser = GlobalTestingContext.BrowserManager.GetBrowser(BrowserOptions, GlobalTestingContext.ConnectionManager.CurrentUserDetails, GlobalTestingContext.ConnectionManager.Url);
            browser.ChangeApp(CurrentApp);
            return browser;
        }

        
    }
}
