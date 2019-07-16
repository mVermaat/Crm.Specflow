using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class UCIBrowser
    {
        private UCIApp _app;
        private string _appId;
        private string _appName;
        private bool _isDisposed = false;

        private Dictionary<string, FormData> _forms;


        static UCIBrowser()
        {
          
        }

        public UCIBrowser(BrowserOptions browserOptions, ButtonTexts buttonTexts)
        {
            _app = new UCIApp(browserOptions, buttonTexts);
            _forms = new Dictionary<string, FormData>();
            _appId = null;
        }

        public void Login(Uri uri, UserDetails connectionString)
        {
            Logger.WriteLine("Logging in CRM");
            if(bool.Parse(HelperMethods.GetAppSettingsValue("UCIOnly")))
            {
                Elements.Xpath[Reference.Login.CrmMainPage] = "//*[@data-id='topBar']";
                AppElements.Xpath[AppReference.Navigation.AppMenuButton] = "//button[@data-id='navbar-switch-app']";
            }

            _app.App.OnlineLogin.Login(uri, connectionString.Username.ToSecureString(), connectionString.Password.ToSecureString());
        }

        public void ChangeApp(string appName)
        {
            if (appName != _appName)
            {
                Logger.WriteLine($"Changing app from {_appName} to {appName}");
                _app.App.Navigation.OpenApp(appName);
                var queryDic = System.Web.HttpUtility.ParseQueryString(new Uri(_app.WebDriver.Url).Query);
                _appId = queryDic["appid"];
                _appName = appName;
                Logger.WriteLine($"Logged into app: {appName} ({_appId})");
            }
            else
            {
                Logger.WriteLine($"App name is already {_appName}. No need to switch");
            }
        }

        public FormData OpenRecord(EntityMetadata entityMetadata, string entityName, Guid? id = null, EntityReference parent = null)
        {
            Logger.WriteLine($"Opening record {entityName} with ID {id}");
            _app.Client.Execute(BrowserOptionHelper.GetOptions($"Open: {entityName}"), driver =>
            {
                Uri uri = new Uri(_app.WebDriver.Url);
                string link = $"{uri.Scheme}://{uri.Authority}/main.aspx?etn={entityName}&pagetype=entityrecord";

                if (id.HasValue)
                    link += $"&id=%7B{id:D}%7D";
                if (!string.IsNullOrEmpty(_appId))
                    link += $"&appid={_appId}";

                if (parent != null)
                {
                    link += $"&extraqs={HttpUtility.UrlEncode($"parentrecordid={parent.Id}&parentrecordname={parent.Name}&parentrecordtype={parent.LogicalName}")}";
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

            return GetFormData(entityMetadata, entityName);
        }

        public FormData OpenRecord(EntityMetadata entityMetadata, EntityReference crmRecord)
        {
            return OpenRecord(entityMetadata, crmRecord.LogicalName, crmRecord.Id);
        }

        public FormData GetFormData(EntityMetadata entityMetadata, string entityName)
        {
            var currentFormId = _app.WebDriver.ExecuteScript("return Xrm.Page.ui.formSelector.getCurrentItem().getId()")?.ToString();

            if(!_forms.TryGetValue(entityName+currentFormId, out FormData formData))
            {
                formData = new FormData(_app, entityMetadata);
                _forms.Add(entityName + currentFormId, formData);
            }

            return formData;
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
