using Microsoft.Dynamics365.UIAutomation.Browser;
using PowerPlatform.SpecflowExtensions.Connectivity;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using PowerPlatform.SpecflowExtensions.Interfaces;
using PowerPlatform.SpecflowExtensions.Models;
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
        private ModelApp _currentApp;

        public BrowserSession(BrowserOptions options)
        {
            _app = new SeleniumApp(options);
        }

        internal void Login(ICrmConnection connection)
        {
            Logger.WriteLine("Logging in CRM");
            _app.Login.Login(connection);
        }

        public void ChangeApp(string appUniqueName)
        {
            if (appUniqueName != _currentApp.Name)
            {
                Logger.WriteLine($"Changing app from {_currentApp.Name} to {appUniqueName}");
                _currentApp = GlobalContext.Metadata.GetModelApp(appUniqueName);
                Logger.WriteLine($"Logged into app: {appUniqueName} (ID: {_currentApp})");
            }
            else
            {
                Logger.WriteLine($"App name is already {_currentApp.Name}. No need to switch");
            }
        }

        public IForm OpenRecord(OpenFormOptions formOptions)
        {
            if (_currentApp == null)
                throw new TestExecutionException(Constants.ErrorCodes.APP_UNSELECTED);

            _app.Navigation.OpenRecord(formOptions, _currentApp.Id);

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
