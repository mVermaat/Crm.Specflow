using BoDi;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using PowerPlatform.SpecflowExtensions.Interfaces;
using PowerPlatform.SpecflowExtensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation = PowerPlatform.SpecflowExtensions.EasyRepro.Selenium.Navigation;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Apps
{
    public class CustomerEngagementApp : IBrowserApp
    {
        private WebClient _client;
        private ISeleniumExecutor _executor;
        private ModelApp _currentApp;

        public INavigation Navigation { get; private set; }


        public void Initialize(WebClient client, ISeleniumExecutor executor)
        {
            _client = client;
            _executor = executor;
            Navigation = new Navigation(_executor);

            var login = new CELogin(_executor, _client);
            login.Login(GlobalContext.ConnectionManager.CurrentConnection);
        }

        public void Refresh(IObjectContainer container)
        {
            var seleniumContext = container.Resolve<ISeleniumContext>();
            ChangeApp(seleniumContext.CurrentApp);
        }

        public IForm GetForm(string entityName)
        {
            return new Form(_executor, entityName);
        }

        public void ChangeApp(string appUniqueName)
        {
            if (appUniqueName != _currentApp?.Name)
            {
                Logger.WriteLine($"Changing app from {_currentApp?.Name} to {appUniqueName}");
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

            Navigation.OpenRecord(formOptions, _currentApp.Id);

            return GetForm(formOptions.EntityName);
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
                    _client = null;
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}
