using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using Vermaat.Crm.Specflow.EasyRepro;
using Vermaat.Crm.Specflow.EasyRepro.Commands;

namespace Vermaat.Crm.Specflow
{
    public class SeleniumTestingContext
    {

        private readonly CrmTestingContext _crmContext;

        public BrowserOptions BrowserOptions { get; }
        public string CurrentApp { get; set; }
        public bool IsLoggedIn { get; set; }
        public SeleniumCommandFactory SeleniumCommandFactory { get; set; }

        public SeleniumTestingContext(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
            BrowserOptions = new BrowserOptions()
            {
                CleanSession = true,
                DriversPath = null,
                StartMaximized = true,
                PrivateMode = true,
                UCITestMode = true,
                CookieСontrolsMode = 0
            };
            CurrentApp = HelperMethods.GetAppSettingsValue("AppName", true);
            BrowserOptions.Headless = Convert.ToBoolean(HelperMethods.GetAppSettingsValue("Headless", true, "false"));
            SeleniumCommandFactory = new SeleniumCommandFactory();
        }

        public void EndCurrentBrowserSession()
        {
            GlobalTestingContext.BrowserManager.EndSession(BrowserOptions, GlobalTestingContext.ConnectionManager.CurrentBrowserLoginDetails);
        }

        public UCIBrowser GetBrowser()
        {
            if (_crmContext.IsTarget("API"))
                throw new TestExecutionException(Constants.ErrorCodes.CANT_START_BROWSER_FOR_API_TESTS);

            var browser = GlobalTestingContext.BrowserManager.GetBrowser(BrowserOptions, GlobalTestingContext.ConnectionManager.CurrentBrowserLoginDetails, SeleniumCommandFactory);
            browser.ChangeApp(CurrentApp);
            IsLoggedIn = true;
            return browser;
        }

    }
}
