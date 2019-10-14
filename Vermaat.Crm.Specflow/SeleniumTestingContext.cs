using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public bool IsLoggedIn { get; private set; }

        public SeleniumTestingContext(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
            BrowserOptions = new BrowserOptions()
            {
                CleanSession = true,
                StartMaximized = true,
                UCITestMode = true,
                DriversPath = GetDriverPath()

            };
            CurrentApp = HelperMethods.GetAppSettingsValue("AppName", true);

        }

        public UCIBrowser GetBrowser()
        {
            if (_crmContext.IsTarget("API"))
                throw new TestExecutionException(Constants.ErrorCodes.CANT_START_BROWSER_FOR_API_TESTS);

            var browser = GlobalTestingContext.BrowserManager.GetBrowser(BrowserOptions, GlobalTestingContext.ConnectionManager.CurrentUserDetails, GlobalTestingContext.ConnectionManager.Url);
            browser.ChangeApp(CurrentApp);
            IsLoggedIn = true;
            return browser;
        }

        private string GetDriverPath()
        {
            var assemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
            Logger.WriteLine($"Using chrome driver path: {assemblyPath.Directory}");
            return assemblyPath.DirectoryName;
        }

    }
}
