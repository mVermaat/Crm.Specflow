using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class UCIApp : IDisposable
    {
        public UCIApp(BrowserOptions options, ButtonTexts buttonTexts)
        {
            Client = new WebClient(options);
            App = new XrmApp(Client);
            ButtonTexts = buttonTexts;
        }

        public XrmApp App { get; }
        public WebClient Client { get; }
        public ButtonTexts ButtonTexts { get; }
        public IWebDriver WebDriver => Client?.Browser.Driver;

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    App.Dispose();
                }

                disposedValue = true;
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
