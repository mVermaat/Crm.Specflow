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

       

        public void FillForm(ICrmContext crmContext, Table tableWithDefaults)
        {
            Logger.WriteLine($"Filling form");
            var formState = new FormState(_app);
            foreach (var row in formData.Rows)
            {
                Assert.IsTrue(ContainsField(row[Constants.SpecFlow.TABLE_KEY]), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                var field = _formFields[row[Constants.SpecFlow.TABLE_KEY]];
                Assert.IsTrue(field.IsVisible(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't visible");
                Assert.IsFalse(field.IsLocked(formState), $"Field {row[Constants.SpecFlow.TABLE_KEY]} is read-only");

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
