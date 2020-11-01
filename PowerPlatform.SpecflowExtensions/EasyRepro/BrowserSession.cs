using BoDi;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
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
        private readonly WebClient _client;
        private readonly XrmApp _xrmApp;
        private readonly ISeleniumExecutor _executor;
        private readonly Dictionary<Type, IBrowserApp> _apps;

        public BrowserSession(BrowserOptions options)
        {
            _client = new WebClient(options);
            _xrmApp = new XrmApp(_client);
            _executor = new SeleniumExecutor(_client, _xrmApp);
            _apps = new Dictionary<Type, IBrowserApp>();
        }

        public T GetApp<T>(IObjectContainer container) where T : IBrowserApp
        {
            if(!_apps.TryGetValue(typeof(T), out var browserApp))
            {
                browserApp = (T)Activator.CreateInstance(typeof(T));
                browserApp.Initialize(_client, _executor);
                _apps.Add(typeof(T), browserApp);
            }

            browserApp.Refresh(container);
            return (T)browserApp;
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
                    foreach(var app in _apps)
                    {
                        app.Value.Dispose();
                    }
                    _xrmApp.Dispose();
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}
