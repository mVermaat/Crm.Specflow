using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Web
{
    class WebBrowser : IBrowser
    {
        private readonly Browser _browser;
        private bool _isDisposed = false;

        public IBrowserEntity Entity { get; }

        public WebBrowser(BrowserOptions browserOptions, ButtonTexts buttonTexts)
        {
            _browser = new Browser(browserOptions);
            Entity = new WebBrowserEntity(this, _browser, buttonTexts);
        }

        public void Login(CrmConnectionString connectionString)
        {
            _browser.LoginPage.Login(new Uri(
               connectionString.Url),
               connectionString.Username.ToSecureString(),
               connectionString.Password.ToSecureString());
        }

        public void OpenNewForm(string entityName)
        {
            var currentUri = new Uri(_browser.Driver.Url);
            var newRecordFormUri = new Uri($"{currentUri.Scheme}://{currentUri.Authority}/main.aspx?etn={entityName}&pagetype=entityrecord");
            _browser.Entity.OpenEntity(newRecordFormUri);
        }

        public void OpenRecord(EntityReference crmRecord)
        {
            OpenRecord(crmRecord.LogicalName, crmRecord.Id);
        }

        public void OpenRecord(string logicalName, Guid id)
        {
            _browser.Entity.OpenEntity(logicalName, id);
        }

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
                    _browser.Dispose();
                }

                _isDisposed = true;
            }
        }

       
    }
}
