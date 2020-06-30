using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium.Support.Extensions;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var formState = new FormState();
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

        private Guid GetCurrentFormId()
        {
            return _executor.Execute("Get Form ID", (driver, selectors) =>
            {
                return driver.ExecuteJavaScript<Guid>(Constants.JavaScriptCommands.GET_FORM_ID);
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

    }
}
