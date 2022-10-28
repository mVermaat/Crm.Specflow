using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Connectivity;
using Vermaat.Crm.Specflow.EasyRepro.Commands;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    internal class BrowserManager : IDisposable
    {
        private readonly LocalizedTexts _localizedTexts;
        private readonly Dictionary<BrowserType, Dictionary<string, UCIBrowser>> _browserCache;
        private readonly Lazy<CrmModelApps> _appCache;

        public BrowserManager(LocalizedTexts buttonTexts)
        {
            _browserCache = new Dictionary<BrowserType, Dictionary<string, UCIBrowser>>();
            _localizedTexts = buttonTexts;
            _appCache = new Lazy<CrmModelApps>(InitializeCache);
        }

        internal void EndSession(BrowserOptions options, BrowserLoginDetails browserLoginDetails)
        {
            Logger.WriteLine($"Ending {options.BrowserType} sesion for {browserLoginDetails.Username}");
            if (_browserCache.TryGetValue(options.BrowserType, out var dic))
            {
                if (dic.TryGetValue(browserLoginDetails.Username, out UCIBrowser browser))
                {
                    dic.Remove(browserLoginDetails.Username);
                    browser.Dispose();
                }
                else
                    Logger.WriteLine("No session exists.");
            }
            else
                Logger.WriteLine("No session exists.");
        }

        public UCIBrowser GetBrowser(BrowserOptions options, BrowserLoginDetails browserLoginDetails, SeleniumCommandFactory seleniumCommandFactory)
        {
            Logger.WriteLine("Getting Browser");
            if(!_browserCache.TryGetValue(options.BrowserType, out var dic))
            {
                Logger.WriteLine($"No browser for {options.BrowserType} doesn't exist. Creating new list");
                dic = new Dictionary<string, UCIBrowser>();
                _browserCache.Add(options.BrowserType, dic);
            }

            if(!dic.TryGetValue(browserLoginDetails.Username, out UCIBrowser browser))
            {
                Logger.WriteLine($"Browser for {browserLoginDetails.Username} doesn't exist. Creating new browser session");

                if(string.IsNullOrEmpty(options.DriversPath))
                {
                    options.DriversPath = GetDriverPath(options);
                }
                
                browser = new UCIBrowser(options, _localizedTexts, _appCache.Value, seleniumCommandFactory);
                dic.Add(browserLoginDetails.Username, browser);
                browser.Login(browserLoginDetails);
            }
            browser.App.SeleniumCommandFactory = seleniumCommandFactory;
            return browser;
        }

        private CrmModelApps InitializeCache()
        {
            Logger.WriteLine("Initializing App Cache");
            return CrmModelApps.GetApps();
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

        #region IDisposable Support
        private bool _disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                Logger.WriteLine("Cleaning up Browser sessions");
                if (disposing)
                {
                    foreach(var list in _browserCache.Values)
                    {
                        foreach(var item in list.Values)
                        {
                            item.Dispose();
                        }
                        list.Clear();
                    }
                    _browserCache.Clear();
                }

                _disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        
        #endregion
    }
}
