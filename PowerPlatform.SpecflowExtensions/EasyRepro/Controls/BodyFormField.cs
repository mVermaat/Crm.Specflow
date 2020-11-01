using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using PowerPlatform.SpecflowExtensions.EasyRepro.FieldTypes;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Controls
{
    internal class BodyFormField : FormField
    {
        private string _tabLabel;

        public BodyFormField(ISeleniumExecutor executor, AttributeMetadata attributeMetadata, string control)
            : base(executor, attributeMetadata, control)
        {
        }

        public override bool IsVisible(FormState formState)
        {
            formState.ExpandTab(GetTabLabel());
            return base.IsVisible(formState);
        }

        public override RequiredState GetRequiredState(FormState formState)
        {
            formState.ExpandTab(GetTabLabel());
            return base.GetRequiredState(formState);
        }

        public override bool IsLocked(FormState formState)
        {
            formState.ExpandTab(GetTabLabel());
            return base.IsLocked(formState);
        }

        private string GetTabLabel()
        {
            return Executor.Execute("Get Tab Label", (driver, selectors) =>
            {
                if (string.IsNullOrEmpty(_tabLabel))
                {
                    //TODO: To constants
                    _tabLabel = driver.ExecuteScript($"return Xrm.Page.getControl('{ControlName}').getParent().getParent().getLabel()")?.ToString();
                }
                return _tabLabel;
            });
        }

        protected override void SetMultiSelectOptionSetField(MultiSelectOptionSetValue value)
        {
            Executor.Execute("Set Multi Select OptionSet", (driver, selectors, app) =>
            {
                app.Entity.SetValue(value.ToMultiValueOptionSet(LogicalName), true);
                return true;
            });
        }

        protected override void SetIntegerField(IntegerValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetDoubleField(DoubleValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetDecimalField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetLongField(LongValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetTwoOptionField(BooleanValue value)
        {
            Executor.Execute("Set Boolean Field", (driver, selectors, app) =>
            {
                app.Entity.SetValue(value.ToBooleanItem(Metadata.LogicalName));
                return true;
            });
        }

        protected override void SetDateTimeField(DateTimeValue value)
        {
            Executor.Execute("Set DateTime Field", (driver, selectors) =>
            {
                TemporaryFixes.SetDateTimeValue(driver, selectors, LogicalName, value.Value,
                 GlobalContext.ConnectionManager.CurrentCrmService.UserSettings.DateFormat,
                 GlobalContext.ConnectionManager.CurrentCrmService.UserSettings.TimeFormat);
                return true;
            });
        }

        protected override void SetOptionSetField(OptionSetValue value)
        {
            Executor.Execute("Set OptionSet Field", (driver, selectors, app) =>
            {
                if (value.Value.HasValue)
                    app.Entity.SetValue(value.ToOptionSet(LogicalName));
                else
                    app.Entity.ClearValue(value.ToOptionSet(LogicalName));
                return true;
            });

           
        }

        protected override void SetMoneyField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetTextField(string fieldValue)
        {
            Executor.Execute("Set Text Field", (driver, selectors) =>
            {
                TemporaryFixes.SetTextField(driver, selectors, LogicalName, fieldValue, TemporaryFixes.ContainerType.Body);
                return true;
            });           
        }

        protected override void SetLookupValue(LookupValue value)
        {
            Executor.Execute("Set OptionSet Field", (driver, selectors, app) =>
            {
                if (value.Value != null)
                {
                    //TODO: To constants 
                    driver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').setValue([ {{ id: '{value.Value.Id}', name: '{value.Value.Name.Replace("'", @"\'")}', entityType: '{value.Value.LogicalName}' }} ])");
                    driver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').fireOnChange()");
                }
                else
                {
                    app.Entity.ClearValue(value.ToLookupItem(Metadata));
                }
                return true;
            });
        }
    }
}
