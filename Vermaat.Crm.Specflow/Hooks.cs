using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

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

        [BeforeScenario]
        public void SetDefaultConnection()
        {
            var authType = HelperMethods.GetAppSettingsValue("AuthType", true);
            dynamic connectionStringHelper;
            switch (authType)
            {
                default:
                    connectionStringHelper = new DefaultConnectionStringHelper();
                    break;
                case "ClientSecret":
                    connectionStringHelper = new AppConnectionStringHelper();
                    break;
            }
           
            if (connectionStringHelper.IsValid())
            {
                GlobalTestingContext.ConnectionManager.SetAdminConnection(connectionStringHelper);
                GlobalTestingContext.ConnectionManager.SetCurrentConnection(connectionStringHelper);
            }
        }

        [AfterScenario("Cleanup")]
        public void Cleanup()
        {
            _crmContext.RecordCache.DeleteAllCachedRecords(GlobalTestingContext.ConnectionManager.AdminConnection);
        }


        [AfterScenario]
        public void AfterWebTest()
        {
            if (_scenarioContext.TestError != null && !_crmContext.IsTarget("API") && _seleniumContext.IsLoggedIn)
            {
                TakeScreenshot(_seleniumContext.GetBrowser().App.WebDriver);
            }
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
