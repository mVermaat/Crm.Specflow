using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using Vermaat.Crm.Specflow.EasyRepro.FieldTypes;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    public class BodyFormField : FormField
    {
        private readonly string _tabName;
        private readonly string _sectionName;

        public BodyFormField(UCIApp app, AttributeMetadata attributeMetadata, FormControl control, string tabName, string sectionName)
            : base(app, attributeMetadata, control)
        {
            _tabName = tabName;
            _sectionName = sectionName;
        }

        public override bool IsVisible(FormState formState)
        {
            try
            {
                formState.ExpandTab(_tabName);
            }
            catch (Exception ex)
            { // Unfortunately easyrepro throws an exception of type system.exception. Can't make this more specific
                Logger.WriteLine("Error expanding tab. Field invisible");
                Logger.WriteLine(ex.Message);
                return false;
            }

            return base.IsVisible(formState);
        }

        public override RequiredState GetRequiredState(FormState formState)
        {
            formState.ExpandTab(_tabName);
            return base.GetRequiredState(formState);
        }

        public override bool IsLocked(FormState formState)
        {
            formState.ExpandTab(_tabName);
            return base.IsLocked(formState);
        }


        protected override void SetMultiSelectOptionSetField(MultiSelectOptionSetValue value)
        {
            App.App.Entity.SetValue(value.ToMultiValueOptionSet(LogicalName), true);
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
            App.App.Entity.SetValue(value.ToBooleanItem(Metadata.LogicalName));
        }

        protected override void SetDateTimeField(DateTimeValue value)
        {
            SeleniumCommandProcessor.ExecuteCommand(App, App.SeleniumCommandFactory.CreateSetDateTimeFieldValueCommand(
                LogicalName, value.Value, value.DateOnly,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.DateFormat,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeFormat));
        }

        protected override void SetOptionSetField(OptionSetValue value)
        {
            if (value.Value.HasValue)
                App.App.Entity.SetValue(value.ToOptionSet(LogicalName));
            else
                App.App.Entity.ClearValue(value.ToOptionSet(LogicalName));
        }

        protected override void SetMoneyField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetTextField(string fieldValue)
        {
            App.Client.SetValueFix(LogicalName, fieldValue, ContainerType.Body);
        }

        protected override void SetLookupValue(LookupValue value)
        {
            if (value.Value != null)
            {
                App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').setValue([ {{ id: '{value.Value.Id}', name: '{value.Value.Name?.Replace("'", @"\'")}', entityType: '{value.Value.LogicalName}' }} ])");
                App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').fireOnChange()");
            }
            else
            {
                SeleniumCommandProcessor.ExecuteCommand(App, App.SeleniumCommandFactory.CreateClearLookupValueCommand(value.ToLookupItem(Metadata)));
            }
        }

        protected override void SetLookupValues(LookupValue[] values)
        {
            var lookupValues = new List<string>();
            foreach (var value in values)
            {
                if (value.Value != null)
                {
                    lookupValues.Add($"{{ id: '{value.Value.Id}', name: '{value.Value.Name?.Replace("'", @"\'")}', entityType: '{value.Value.LogicalName}' }}");
                }
            }

            if (lookupValues.Count > 0)
            {
                App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').setValue([ {string.Join(", ", lookupValues)} ])");
                App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').fireOnChange()");
            }
            else
            {
                App.App.Entity.ClearValue(new LookupItem() { Name = Metadata.LogicalName });
            }
        }
    }
}
