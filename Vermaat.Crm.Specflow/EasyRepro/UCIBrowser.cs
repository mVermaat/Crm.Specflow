using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using Vermaat.Crm.Specflow.Connectivity;
using Vermaat.Crm.Specflow.EasyRepro.Commands;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class UCIBrowser : IDisposable
    {
        private Guid? _currentAppId;
        private string _currentAppName;
        private bool _isDisposed = false;

        private readonly Dictionary<string, FormData> _forms;
        private readonly Dictionary<string, QuickFormData> _quickForms;
        private readonly CrmModelApps _apps;

        public UCIApp App { get; }
        public FormData LastFormData { get; private set; }

        static UCIBrowser()
        {

        }

        public UCIBrowser(BrowserOptions browserOptions, LocalizedTexts localizedTexts, CrmModelApps apps, SeleniumCommandFactory seleniumCommandFactory)
        {
            App = new UCIApp(browserOptions, localizedTexts, seleniumCommandFactory, GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.UILanguage);
            _forms = new Dictionary<string, FormData>();
            _quickForms = new Dictionary<string, QuickFormData>();
            _apps = apps;
        }

        public void Login(BrowserLoginDetails loginDetails)
        {
            Logger.WriteLine("Logging in CRM");
            App.App.OnlineLogin.Login(new Uri(loginDetails.Url), loginDetails.Username.ToSecureString(), loginDetails.Password, loginDetails.MfaKey);
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
                CheckForWavePopup(driver);

                if (App.Client.ScriptErrorExists())
                    throw new TestExecutionException(Constants.ErrorCodes.FORMLOAD_SCRIPT_ERROR_ON_FORM);

                return true;
            });

            return GetFormData(GlobalTestingContext.Metadata.GetEntityMetadata(formOptions.EntityName));
        }

        private void CheckForWavePopup(IWebDriver driver)
        {
            if (driver.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Popup_TeachingBubble_CloseButton), out var closeButton))
            {
                closeButton.Click();
            }
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

            if (!_forms.TryGetValue(entityMetadata.LogicalName + currentFormId, out FormData formData))
            {
                formData = new FormData(App, entityMetadata);
                _forms.Add(entityMetadata.LogicalName + currentFormId, formData);
            }

            LastFormData = formData;
            return formData;
        }

        public QuickFormData GetQuickFormData(EntityMetadata entityMetadata)
        {
            var currentFormId = App.WebDriver.ExecuteScript("return Xrm.Page.ui.formSelector._formId.guid")?.ToString();

            if (!_quickForms.TryGetValue(entityMetadata.LogicalName + currentFormId, out QuickFormData formData))
            {
                formData = new QuickFormData(App, entityMetadata);
                _quickForms.Add(entityMetadata.LogicalName + currentFormId, formData);
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
