using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.UCI
{
    class UCIBrowser : IBrowser
    {
        private WebClient _client;
        private XrmApp _app;
        private bool _isDisposed = false;

        public IBrowserEntity Entity { get; }

        static UCIBrowser()
        {
            Elements.Xpath["Login_CrmMainPage"] = "//*[@data-id='topBar']";
            AppElements.Xpath["Nav_AppMenuButton"] = "//button[@data-id='navbar-switch-app']";
        }

        public UCIBrowser(BrowserOptions browserOptions, ButtonTexts buttonTexts)
        {
            _client = new WebClient(browserOptions);
            _app = new XrmApp(_client);

            Entity = new UCIBrowserEntity(this, _client, _app, buttonTexts);
        }

        public void Login(CrmConnectionString connectionString)
        {
            _app.OnlineLogin.Login(new Uri(connectionString.Url), connectionString.Username.ToSecureString(), connectionString.Password.ToSecureString());
            
        }

        public void OpenNewForm(string entityName)
        {
            var currentUri = new Uri(_client.Browser.Driver.Url);
            var newRecordFormUri = new Uri($"{currentUri.Scheme}://{currentUri.Authority}/main.aspx?etn={entityName}&pagetype=entityrecord");
            _client.Browser.Navigate(newRecordFormUri);
        }

        public void OpenRecord(EntityReference crmRecord)
        {
            OpenRecord(crmRecord.LogicalName, crmRecord.Id);
        }

        public void OpenRecord(string logicalName, Guid id)
        {
            _app.Entity.OpenEntity(logicalName, id);
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
                    _app.Dispose();
                }

                _isDisposed = true;
            }
        }

        
    }
}
