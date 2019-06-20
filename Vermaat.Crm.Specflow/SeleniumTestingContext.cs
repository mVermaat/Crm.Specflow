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
        private readonly string[] _targets;

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

            _targets = ConfigurationManager.AppSettings["Target"]
                .ToLower()
                .Split(';')
                .Select(splitted => splitted.Trim())
                .ToArray();
        }

        public UCIBrowser GetBrowser()
        {
            var browser = GlobalTestingContext.BrowserManager.GetBrowser(BrowserOptions, GlobalTestingContext.ConnectionManager.CurrentUserDetails, GlobalTestingContext.ConnectionManager.Url);
            browser.ChangeApp(CurrentApp);
            return browser;
        }

        public bool IsTarget(string target)
        {
            if (string.IsNullOrWhiteSpace(target))
                return false;

            return _targets.Contains(target.ToLower());
        }
    }
}
