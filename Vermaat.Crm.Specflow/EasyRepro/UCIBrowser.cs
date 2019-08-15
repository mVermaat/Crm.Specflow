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
    public class UCIBrowser : IDisposable
    {
        private string _appId;
        private string _appName;
        private bool _isDisposed = false;

        private Dictionary<string, FormData> _forms;

        public UCIApp App { get; }
        public FormData LastFormData { get; private set; }

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

        public FormData OpenRecord(OpenFormOptions formOptions)
        {
            Logger.WriteLine($"Opening record {formOptions.EntityName} with ID {formOptions.EntityId}");
            App.Client.Execute(BrowserOptionHelper.GetOptions($"Open: {formOptions.EntityName}"), driver =>
            {

                driver.Navigate().GoToUrl(formOptions.GetUrl(driver, _appId));
                CheckAlert(driver);
                HelperMethods.WaitForFormLoad(driver);

                if (App.Client.ScriptErrorExists())
                    throw new TestExecutionException(Constants.ErrorCodes.FORMLOAD_SCRIPT_ERROR_ON_FORM);

                return true;
            });

            return GetFormData(GlobalTestingContext.Metadata.GetEntityMetadata(formOptions.EntityName));
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

        public FormData GetFormData(EntityMetadata entityMetadata)
        {
            var currentFormId = App.WebDriver.ExecuteScript("return Xrm.Page.ui.formSelector.getCurrentItem().getId()")?.ToString();

            if(!_forms.TryGetValue(entityMetadata.LogicalName + currentFormId, out FormData formData))
            {
                formData = new FormData(App, entityMetadata);
                _forms.Add(entityMetadata.LogicalName + currentFormId, formData);
            }

            LastFormData = formData;
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
