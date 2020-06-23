using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    public class SeleniumApp : IDisposable
    {
        private WebClient _client;
        private XrmApp _app;

        public ILogin Login { get; }

        public SeleniumApp(BrowserOptions options)
        {
            _client = new WebClient(options);
            _app = new XrmApp(_client);

            // Todo: make implementers override this:
            Login = new CELogin(_client);
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
