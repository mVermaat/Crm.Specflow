using Microsoft.Dynamics365.UIAutomation.Browser;
using PowerPlatform.SpecflowExtensions.Connectivity;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro
{
    public class BrowserSession : IDisposable
    {
        private SeleniumApp _app;

        public BrowserSession(BrowserOptions options)
        {
            _app = new SeleniumApp(options);
        }

        internal void Login(ICrmConnection connection)
        {
            Logger.WriteLine("Logging in CRM");
            _app.Login.Login(connection);

        }

        #region IDisposable

        private bool _isDisposed = false;

        public void Dispose()
        {
            Dispose(true);
        }

        
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _app.Dispose();
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}
