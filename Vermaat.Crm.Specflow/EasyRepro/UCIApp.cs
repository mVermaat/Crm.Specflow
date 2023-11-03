using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using Vermaat.Crm.Specflow.EasyRepro.Commands;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class UCIApp : IDisposable
    {
        public UCIApp(BrowserOptions options, LocalizedTexts localizedTexts, SeleniumCommandFactory seleniumCommandFactory, int uiLanguageCode)
        {
            Client = new WebClient(options);
            App = new XrmApp(Client);
            LocalizedTexts = localizedTexts;
            UILanguageCode = uiLanguageCode;
            SeleniumCommandFactory = seleniumCommandFactory;
        }

        public XrmApp App { get; }
        public WebClient Client { get; }
        public LocalizedTexts LocalizedTexts { get; }
        public SeleniumCommandFactory SeleniumCommandFactory { get; set; }
        public int UILanguageCode { get; }
        public IWebDriver WebDriver => Client?.Browser.Driver;

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    App.Dispose();
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
