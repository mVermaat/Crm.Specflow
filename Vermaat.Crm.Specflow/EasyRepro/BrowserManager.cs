using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class BrowserManager : IDisposable
    {
        private readonly ButtonTexts _buttonTexts;
        private Dictionary<BrowserType, Dictionary<string, UCIBrowser>> _browserCache;

        public BrowserManager(ButtonTexts buttonTexts)
        {
            _browserCache = new Dictionary<BrowserType, Dictionary<string, UCIBrowser>>();
            _buttonTexts = buttonTexts;
        }

        public UCIBrowser GetBrowser(BrowserOptions options, UserDetails userDetails, Uri uri)
        {
            Logger.WriteLine("Getting Browser");
            if(!_browserCache.TryGetValue(options.BrowserType, out var dic))
            {
                Logger.WriteLine($"No browser for {options.BrowserType} doesn't exist. Creating new list");
                dic = new Dictionary<string, UCIBrowser>();
                _browserCache.Add(options.BrowserType, dic);
            }

            if(!dic.TryGetValue(userDetails.Username, out UCIBrowser browser))
            {
                Logger.WriteLine($"Browser for {userDetails.Username} doesn't exist. Creating new browser session");

                browser = new UCIBrowser(options, _buttonTexts);
                dic.Add(userDetails.Username, browser);
                browser.Login(uri, userDetails);
            }
            return browser;
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
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

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BrowserManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
