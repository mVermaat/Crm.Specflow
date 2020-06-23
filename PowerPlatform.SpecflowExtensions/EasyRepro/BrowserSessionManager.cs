using Microsoft.Dynamics365.UIAutomation.Browser;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro
{
    internal class BrowserSessionManager
    {
        private readonly Dictionary<BrowserType, Dictionary<string, BrowserSession>> _browserCache;


        public BrowserSessionManager()
        {
            _browserCache = new Dictionary<BrowserType, Dictionary<string, BrowserSession>>();
        }

        public BrowserSession GetBrowserSession(BrowserOptions options, ICrmConnection connection)
        {
            Logger.WriteLine("Getting Browser");
            if (!_browserCache.TryGetValue(options.BrowserType, out var dic))
            {
                Logger.WriteLine($"No browser for {options.BrowserType} doesn't exist. Creating new list");
                dic = new Dictionary<string, BrowserSession>();
                _browserCache.Add(options.BrowserType, dic);
            }

            if (!dic.TryGetValue(connection.Identifier, out BrowserSession browser))
            {
                Logger.WriteLine($"Browser for {connection.Identifier} doesn't exist. Creating new browser session");

                if (string.IsNullOrEmpty(options.DriversPath))
                {
                    options.DriversPath = GetDriverPath(options);
                }

                browser = new BrowserSession();
                dic.Add(connection.Identifier, browser);
                browser.Login(connection);
            }
            return browser;
        }

        private string GetDriverPath(BrowserOptions options)
        {
            string envWebDriver = null;
            string driverFile = null;
            switch (options.BrowserType)
            {
                case BrowserType.Chrome:
                    envWebDriver = Environment.GetEnvironmentVariable("ChromeWebDriver");
                    driverFile = "chromedriver.exe";
                    break;
                case BrowserType.Firefox:
                    envWebDriver = Environment.GetEnvironmentVariable("GeckoWebDriver");
                    driverFile = "geckodriver.exe";
                    break;
                case BrowserType.IE:
                    envWebDriver = Environment.GetEnvironmentVariable("IEWebDriver");
                    driverFile = "IEDriverServer.exe";
                    break;
            }

            if (!string.IsNullOrEmpty(envWebDriver) && File.Exists(Path.Combine(envWebDriver, driverFile)))
            {
                Logger.WriteLine($"Using driver path via environmentvariable. Driver path: {envWebDriver}");
                return envWebDriver;
            }
            else
            {
                var assemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
                Logger.WriteLine($"Using chrome driver path: {assemblyPath.Directory}");
                return assemblyPath.DirectoryName;
            }
        }
    }
}
