using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    internal class SeleniumApp : IDisposable
    {
        private WebClient _client;
        private XrmApp _app;
        private SeleniumExecutor _executor;

        public ILogin Login { get; }
        public INavigation Navigation { get; }


        public SeleniumApp(BrowserOptions options)
        {
            _client = new WebClient(options);
            _app = new XrmApp(_client);
            _executor = new SeleniumExecutor(_client);

            // Todo: make implementers override this:
            Login = new CELogin(_executor, _client);
            Navigation = new Navigation(_executor);
        }

        public Form GetForm(string entityName)
        {
            return new Form(_executor, entityName);
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
                    _client = null;
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}
