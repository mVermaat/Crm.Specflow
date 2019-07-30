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
        private string _appId;
        private string _appName;
        private bool _isDisposed = false;

        private Dictionary<string, FormData> _forms;

        public UCIApp App { get; }

        static UCIBrowser()
        {
          
        }

        public UCIBrowser(BrowserOptions browserOptions, ButtonTexts buttonTexts)
        {
            App = new UCIApp(browserOptions, buttonTexts);
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

            App.App.OnlineLogin.Login(uri, connectionString.Username.ToSecureString(), connectionString.Password.ToSecureString());
        }

        public void ChangeApp(string appName)
        {
            if (appName != _appName)
            {
                Logger.WriteLine($"Changing app from {_appName} to {appName}");
                App.App.Navigation.OpenApp(appName);
                var queryDic = System.Web.HttpUtility.ParseQueryString(new Uri(App.WebDriver.Url).Query);
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
            App.Client.Execute(BrowserOptionHelper.GetOptions($"Open: {entityName}"), driver =>
            {
                Uri uri = new Uri(App.WebDriver.Url);
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
                CheckAlert(driver);
                HelperMethods.WaitForFormLoad(driver);

                if (App.Client.ScriptErrorExists())
                    throw new TestExecutionException(Constants.ErrorCodes.FORMLOAD_SCRIPT_ERROR_ON_FORM);

                return true;
            });

            return GetFormData(entityMetadata, entityName);
        }

        private void CheckAlert(IWebDriver driver)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                alert.Accept();
            }   
            catch (NoAlertPresentException)
            {
            }
        }

        public FormData OpenRecord(EntityMetadata entityMetadata, EntityReference crmRecord)
        {
            return OpenRecord(entityMetadata, crmRecord.LogicalName, crmRecord.Id);
        }

        public FormData GetFormData(EntityMetadata entityMetadata, string entityName)
        {
            var currentFormId = App.WebDriver.ExecuteScript("return Xrm.Page.ui.formSelector.getCurrentItem().getId()")?.ToString();

            if(!_forms.TryGetValue(entityName+currentFormId, out FormData formData))
            {
                formData = new FormData(App, entityMetadata);
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
                    App.Dispose();
                }

                _isDisposed = true;
            }
        }


    }
}
