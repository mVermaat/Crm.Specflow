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
    internal class HeaderFormField : FormField
    {

        public HeaderFormField(SeleniumExecutor executor, AttributeMetadata attributeMetadata, string control)
            : base(executor, attributeMetadata, control)
        {
        }

        public override bool IsVisible(FormState formState)
        {
            formState.ExpandHeader();
            return base.IsVisible(formState);
        }

        protected override void SetDateTimeField(DateTimeValue value)
        {
            App.Client.SetValueFix(LogicalName, value.Value,
                GlobalContext.ConnectionManager.CurrentConnection.UserSettings.DateFormat,
                GlobalContext.ConnectionManager.CurrentConnection.UserSettings.TimeFormat);
        }

        protected override void SetDecimalField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetDoubleField(DoubleValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetIntegerField(IntegerValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetLongField(LongValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetLookupValue(LookupValue value)
        {
            Executor.Execute("Set Lookup Field", (driver, selectors, app) =>
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

        protected override void SetMoneyField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetMultiSelectOptionSetField(MultiSelectOptionSetValue value)
        {
            Executor.Execute("Set Multi Select OptionSet Field", (driver, selectors, app) =>
            {
                app.Entity.SetHeaderValue(value.ToMultiValueOptionSet(LogicalName));
                return true;
            });
        }

        protected override void SetOptionSetField(OptionSetValue value)
        {
            Executor.Execute("Set OptionSet Field", (driver, selectors, app) =>
            {
                if (value.Value.HasValue)
                    app.Entity.SetHeaderValue(value.ToOptionSet(LogicalName));
                else
                    app.Entity.ClearValue(value.ToOptionSet(LogicalName));
                return true;
            });           
        }

        protected override void SetTextField(string fieldValue)
        {
            App.Client.SetValueFix(LogicalName, fieldValue, ContainerType.Header);
        }

        protected override void SetTwoOptionField(BooleanValue value)
        {
            Executor.Execute("Set Boolean Field", (driver, selectors, app) =>
            {
                app.Entity.SetHeaderValue(value.ToBooleanItem(LogicalName));
                return true;
            }); 
        }
    }
}
