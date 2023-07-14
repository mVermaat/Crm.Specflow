using Microsoft.Dynamics365.UIAutomation.Api.UCI;
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
        private readonly Action<LoginRedirectEventArgs> _redirectAction;

        public UCIApp App { get; }
        public FormData LastFormData { get; private set; }

        static UCIBrowser()
        {

        }

        public UCIBrowser(BrowserOptions browserOptions, LocalizedTexts localizedTexts, CrmModelApps apps, SeleniumCommandFactory seleniumCommandFactory,
            Action<LoginRedirectEventArgs> redirectAction = null)
        {
            App = new UCIApp(browserOptions, localizedTexts, seleniumCommandFactory, GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.UILanguage);
            _forms = new Dictionary<string, FormData>();
            _quickForms = new Dictionary<string, QuickFormData>();
            _apps = apps;
            _redirectAction = redirectAction;
        }

        public void Login(BrowserLoginDetails loginDetails)
        {
            Logger.WriteLine("Logging in CRM");
            App.App.OnlineLogin.Login(new Uri(loginDetails.Url), loginDetails.Username.ToSecureString(), loginDetails.Password, loginDetails.MfaKey, _redirectAction);
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
            SeleniumCommandProcessor.ExecuteCommand(App, App.SeleniumCommandFactory.CreateOpenRecordCommand(formOptions, _currentAppId));
            return GetFormData(GlobalTestingContext.Metadata.GetEntityMetadata(formOptions.EntityName));
        }

        public FormData GetFormData(EntityMetadata entityMetadata)
        {
            var currentForm = SeleniumCommandProcessor.ExecuteCommand(App, App.SeleniumCommandFactory.CreateGetCurrentFormCommand(false));
            var currentFormId = currentForm.Id.ToString();

            if (!_forms.TryGetValue(entityMetadata.LogicalName + currentFormId, out FormData formData))
            {
                formData = new FormData(App, entityMetadata, currentForm);
                _forms.Add(entityMetadata.LogicalName + currentFormId, formData);
            }

            LastFormData = formData;
            return formData;
        }

        public QuickFormData GetQuickFormData(EntityMetadata entityMetadata)
        {
            var currentForm = SeleniumCommandProcessor.ExecuteCommand(App, App.SeleniumCommandFactory.CreateGetCurrentFormCommand(true));
            var currentFormId = currentForm.Id.ToString();

            if (!_quickForms.TryGetValue(entityMetadata.LogicalName + currentFormId, out QuickFormData formData))
            {
                formData = new QuickFormData(App, entityMetadata, currentForm);
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
