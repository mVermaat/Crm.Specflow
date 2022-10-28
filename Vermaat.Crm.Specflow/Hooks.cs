using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Connectivity;

namespace Vermaat.Crm.Specflow
{
    [Binding]
    public class Hooks
    {
        private readonly SeleniumTestingContext _seleniumContext;
        private readonly CrmTestingContext _crmContext;
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;

        public Hooks(SeleniumTestingContext seleniumTestingContext, CrmTestingContext crmContext,
                     FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _seleniumContext = seleniumTestingContext;
            _crmContext = crmContext;
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        /// <summary>
        /// This is a temporary fix as due to another issue EasyRepro can't be updated right now
        /// </summary>
        [BeforeScenario]
        public void FixXPaths()
        {
            AppElements.Xpath[AppReference.Entity.TabList] = "//ul[contains(@id, \"tablist\")]";
        }

        [BeforeScenario("API")]
        public void APISetup()
        {

        }

        [BeforeScenario("Chrome")]
        public void ChromeSetup()
        {
            if (_crmContext.IsTarget("Chrome"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.Chrome;
            }
        }

        [BeforeScenario("Edge")]
        public void EdgeSetup()
        {
            if (_crmContext.IsTarget("Edge"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.Edge;
            }
        }

        [BeforeScenario("Firefox")]
        public void FirefoxSetup()
        {
            if (_crmContext.IsTarget("Firefox"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.Firefox;
            }
        }

        [BeforeScenario("IE")]
        public void IESetup()
        {
            if (_crmContext.IsTarget("IE"))
            {
                _seleniumContext.BrowserOptions.BrowserType = BrowserType.IE;
            }
        }

        [BeforeScenario("ForceAPI")]
        public void ForceApi()
        {
            _crmContext.CommandProcessor.DefaultCommandAction = CommandAction.ForceApi;
        }

        [BeforeScenario("ForceBrowser")]
        public void ForceBrowser()
        {
            _crmContext.CommandProcessor.DefaultCommandAction = CommandAction.ForceBrowser;
        }

        [BeforeScenario("PreferAPI")]
        public void PreferApi()
        {
            _crmContext.CommandProcessor.DefaultCommandAction = CommandAction.PreferApi;
        }

        [BeforeScenario("PreferBrowser")]
        public void PreferBrowser()
        {
            _crmContext.CommandProcessor.DefaultCommandAction = CommandAction.PreferBrowser;
        }


        [BeforeScenario]
        public void SetDefaultConnection()
        {
            // Fallback to 'Default' for backwards compatibility
            var loginType = HelperMethods.GetAppSettingsValue("LoginType", true) ?? "Default";

            CrmConnection connection;
            CrmConnection adminConnection;
            switch (loginType)
            {
                case "Default":
                    connection = OAuthCrmConnection.FromAppConfig();
                    adminConnection = OAuthCrmConnection.AdminConnectionFromAppConfig();
                    break;
                case "ClientSecret":
                    connection = ClientSecretCrmConnection.CreateFromAppConfig();
                    adminConnection = ClientSecretCrmConnection.CreateAdminConnectionFromAppConfig();
                    break;
                case "OAuth":
                    connection = OAuthCrmConnection.FromAppConfig();
                    adminConnection = OAuthCrmConnection.AdminConnectionFromAppConfig();
                    break;
                case "Hybrid":
                    connection = HybridCrmConnection.CreateFromAppConfig();
                    adminConnection = HybridCrmConnection.CreateAdminConnectionFromAppConfig();
                    break;
                // Implementations can add their own 'LoginType'. If this is done, then this method shouldn't do anything
                default: 
                    return;

            }

            GlobalTestingContext.ConnectionManager.SetAdminConnection(adminConnection);
            GlobalTestingContext.ConnectionManager.SetCurrentConnection(connection);

        }

        [AfterScenario("Cleanup")]
        public void Cleanup()
        {
            _crmContext.RecordCache.DeleteAllCachedRecords();
        }


        [AfterScenario]
        public void AfterWebTest()
        {
            if (_scenarioContext.TestError != null && !_crmContext.IsTarget("API") && _seleniumContext.IsLoggedIn)
            {
                TakeScreenshot(_seleniumContext.GetBrowser().App.WebDriver);
            }

            if (_scenarioContext.TestError is WebDriverException)
            {
                _seleniumContext.EndCurrentBrowserSession();
            }
        }

        [BeforeTestRun]
        public static void BeforeTestRunXPathFixes()
        {
            AppElements.Xpath[AppReference.Dialogs.ConfirmButton] = "//button[@data-id='confirmButton']";
            AppElements.Xpath[AppReference.Dialogs.CancelButton] = "//button[@data-id='cancelButton']";
        }

        [AfterTestRun]
        public static void AfterTestRunCleanup()
        {
            GlobalTestingContext.BrowserManager.Dispose();
            GlobalTestingContext.ConnectionManager.Dispose();
        }



        private void TakeScreenshot(IWebDriver driver)
        {
            try
            {
                string fileNameBase = string.Format("error_{0}_{1}_{2}",
                                                    _featureContext.FeatureInfo.Title,
                                                    _scenarioContext.ScenarioInfo.Title,
                                                     DateTime.Now.ToString("yyyyMMdd_HHmmss"))
                                                    .Replace(' ', '_')
                                                    .Replace(":", "");

                var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
                if (!directory.Name.Equals("TestResults", StringComparison.InvariantCultureIgnoreCase))
                {
                    directory = directory.CreateSubdirectory("TestResults");
                }
                Logger.WriteLine($"Screenshot path: {directory.FullName}");

                string pageSource = driver.PageSource;
                string sourceFilePath = Path.Combine(directory.FullName, fileNameBase + "_source.html");
                File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);
                Console.WriteLine("Page source: {0}", new Uri(sourceFilePath));


                if (driver is ITakesScreenshot takesScreenshot)
                {
                    var screenshot = takesScreenshot.GetScreenshot();

                    string screenshotFilePath = Path.Combine(directory.FullName, fileNameBase + "_screenshot.png");

                    screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);

                    Logger.WriteLine($"Screenshot: {new Uri(screenshotFilePath)}");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error while taking screenshot: {ex}");
            }
        }
    }
}
