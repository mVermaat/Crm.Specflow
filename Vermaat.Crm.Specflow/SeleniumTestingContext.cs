using Microsoft.Dynamics365.UIAutomation.Browser;
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
                DriversPath = null,

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

    }
}
