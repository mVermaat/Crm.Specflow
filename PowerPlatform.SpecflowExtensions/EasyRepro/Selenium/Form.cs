using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    class Form : IForm
    {
        private readonly EntityMetadata _entityMetadata;
        private readonly SeleniumExecutor _executor;
        private readonly Guid _formId;
        private readonly FormStructure _formStructure;

        public Form(SeleniumExecutor executor, string entityName)
        {
            _executor = executor;
            _entityMetadata = GlobalContext.Metadata.GetEntityMetadata(entityName);
            _formId = GetCurrentFormId();
            _formStructure = GetFormStructure();

        }

        public void FillForm(ICrmContext crmContext, Table formData)
        {
            Logger.WriteLine($"Filling form");
            var formState = new FormState(_executor);
            foreach (var row in formData.Rows)
            {
                if (!_formStructure.TryGetFormField(row[Constants.SpecFlow.TABLE_KEY], out var field))
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, row[Constants.SpecFlow.TABLE_KEY]);

                if (!field.IsVisible(formState))
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_INVISIBLE, row[Constants.SpecFlow.TABLE_KEY]);

                if (field.IsLocked(formState))
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_READ_ONLY, row[Constants.SpecFlow.TABLE_KEY]);

                field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
            }
        }

        public IReadOnlyCollection<FormNotification> GetFormNotifications()
        {
            return _executor.Execute("Get Form Notifcations", 
                (driver, selectors, app) => app.Entity.GetFormNotifications()); 
        }

        public Guid GetRecordId()
        {
            return _executor.Execute("Get Record ID", (driver, selectors, app) =>
            {
                Logger.WriteLine("Getting Record Id");
                var id = app.Entity.GetObjectId();
                Logger.WriteLine($"Record ID of current opened record: {id}");
                return id;
            });
        }

        public void Save(bool saveIfDuplicate)
        {
            _executor.Execute("Save Form", (driver, selectors, app) =>
            {
                Logger.WriteLine($"Saving Record");
                try
                {
                    app.Entity.Save();
                }
                catch (InvalidOperationException ex)
                {
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_FAILED, ex, ex.Message);
                }

                // Check for duplicates
                var element = driver.WaitUntilAvailable(
                    By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton]),
                    new TimeSpan(0, 0, 5));
                if (element != null)
                {
                    if (saveIfDuplicate)
                    {
                        driver.ClickWhenAvailable(
                            By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton]),
                            new TimeSpan(0, 0, 5));
                    }
                    else
                    {
                        throw new TestExecutionException(Constants.ErrorCodes.DUPLICATE_RECORD_DETECTED);
                    }
                }

                // Wait until completion
                WaitUntilSaveCompleted(driver, app);
                return true;
            });
        }

        private Guid GetCurrentFormId()
        {
            return _executor.Execute("Get Form ID", (driver, selectors) =>
            {
                return Guid.Parse(driver.ExecuteScript(Constants.JavaScriptCommands.GET_FORM_ID).ToString());
            });
        }

        private FormStructure GetFormStructure()
        {
            var structure = GlobalContext.FormStructureCache.GetFormStructure(_entityMetadata.LogicalName, _formId);

            if (structure == null)
            {
                structure = FormStructure.FromCurrentScreen(_executor, _entityMetadata);
                GlobalContext.FormStructureCache.AddFormStructure(_entityMetadata.LogicalName, _formId, structure);
            }

            return structure;
        }

        private void WaitUntilSaveCompleted(IWebDriver driver, XrmApp app)
        {
            var timeout = DateTime.Now.AddSeconds(20);
            bool saveCompleted = false;
            while (!saveCompleted && DateTime.Now < timeout)
            {
                var footerElement = driver.FindElement(By.XPath("//span[@data-id='edit-form-footer-message']"));

                if (!string.IsNullOrEmpty(footerElement.Text) && footerElement.Text.ToLower() == "saving")
                {
                    Logger.WriteLine("Save not yet completed. Waiting..");
                    Thread.Sleep(500);
                }
                else if (!string.IsNullOrEmpty(footerElement.Text) && footerElement.Text.ToLower() == "unsaved changes")
                {
                    var formNotifications = app.Entity.GetFormNotifications();
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_FAILED, $"Detected Unsaved changes. Form Notifications: {string.Join(", ", formNotifications)}");
                }
                else
                {
                    Logger.WriteLine("Save sucessfull");
                    saveCompleted = true;
                }
            }

            if (!saveCompleted)
                throw new TestExecutionException(Constants.ErrorCodes.FORM_SAVE_TIMEOUT, 20);
        }

    }
}
