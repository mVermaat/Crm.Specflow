using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class UCIBrowser : IDisposable
    {
        private Guid? _currentAppId;
        private string _currentAppName;
        private bool _isDisposed = false;

        private readonly Dictionary<string, FormData> _forms;
        private readonly CrmModelApps _apps;

        public UCIApp App { get; }
        public FormData LastFormData { get; private set; }

        static UCIBrowser()
        {
          
        }

        public UCIBrowser(BrowserOptions browserOptions, ButtonTexts buttonTexts, CrmModelApps apps)
        {
            App = new UCIApp(browserOptions, buttonTexts);
            _forms = new Dictionary<string, FormData>();
            _apps = apps;
        }

        public void Login(Uri uri, UserDetails userDetails)
        {
            Logger.WriteLine("Logging in CRM");
            if(bool.Parse(HelperMethods.GetAppSettingsValue("UCIOnly")))
            {
                Elements.Xpath[Reference.Login.CrmMainPage] = "//*[@data-id='topBar']";
                AppElements.Xpath[AppReference.Navigation.AppMenuButton] = "//button[@data-id='navbar-switch-app']";
            }

            App.App.OnlineLogin.Login(uri, userDetails.Username.ToSecureString(), userDetails.Password.ToSecureString());
        }

        public void ChangeApp(string appUniqueName)
        {
            if (appUniqueName != _currentAppName)
            {
                Logger.WriteLine($"Changing app from {_currentAppName} to {appUniqueName}");
                _currentAppId = _apps.GetAppId(appUniqueName);
                _currentAppName = appUniqueName;
                Logger.WriteLine($"Logged into app: {appUniqueName} (ID: {_currentAppId})");
            }
            else
            {
                Logger.WriteLine($"App name is already {_currentAppName}. No need to switch");
            }
        }

        public FormData OpenRecord(OpenFormOptions formOptions)
        {
            Logger.WriteLine($"Opening record {formOptions.EntityName} with ID {formOptions.EntityId}");
            App.Client.Execute(BrowserOptionHelper.GetOptions($"Open: {formOptions.EntityName}"), driver =>
            {

                driver.Navigate().GoToUrl(formOptions.GetUrl(driver, _currentAppId));
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
