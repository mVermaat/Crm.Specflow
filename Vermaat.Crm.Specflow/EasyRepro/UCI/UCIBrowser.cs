using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using System;

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
            _app.Navigation.OpenApp(connectionString.AppName);
        }

        public void OpenRecord(string entityName, Guid? id = null)
        {
            _client.Execute(BrowserOptionHelper.GetOptions($"Open: {entityName}"), driver =>
            {
                Uri uri = new Uri(_client.Browser.Driver.Url);
                string link = $"{uri.Scheme}://{uri.Authority}/main.aspx?etn={entityName}&pagetype=entityrecord";

                if (id.HasValue)
                {
                    link += $"&id=%7B{id:D}%7D";
                }

                driver.Navigate().GoToUrl(link);
                driver.WaitForPageToLoad();
                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Entity.Form]),
                    new TimeSpan(0, 0, 30),
                    null,
                    d => { throw new Exception("CRM Record is Unavailable or not finished loading. Timeout Exceeded"); }
                );

                return true;
            });
        }

        public void OpenRecord(EntityReference crmRecord)
        {
            OpenRecord(crmRecord.LogicalName, crmRecord.Id);
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
