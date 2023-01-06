using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro.FieldTypes;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    class HeaderFormField : FormField
    {


        public HeaderFormField(UCIApp app, AttributeMetadata attributeMetadata, FormControl control) 
            : base(app, attributeMetadata, control)
        {
        }

        public override bool IsVisible(FormState formState)
        {
            formState.ExpandHeader();
            return base.IsVisible(formState);
        }

        protected override void SetDateTimeField(DateTimeValue value)
        {
            App.Client.SetValueFix(LogicalName, value.Value, value.DateOnly, 
                GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.DateFormat,
                GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeFormat);
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
            if (value.Value != null)
            {
                App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').setValue([ {{ id: '{value.Value.Id}', name: '{value.Value.Name.Replace("'", @"\'")}', entityType: '{value.Value.LogicalName}' }} ])");
                App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').fireOnChange()");
            }
            else
            {
                App.App.Entity.ClearValue(value.ToLookupItem(Metadata));
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

        protected override void SetMoneyField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetMultiSelectOptionSetField(MultiSelectOptionSetValue value)
        {
            App.App.Entity.SetHeaderValue(value.ToMultiValueOptionSet(LogicalName));
        }

        protected override void SetOptionSetField(OptionSetValue value)
        {
            if (value.Value.HasValue)
                App.App.Entity.SetHeaderValue(value.ToOptionSet(LogicalName));
            else
                App.App.Entity.ClearValue(value.ToOptionSet(LogicalName));
        }

        protected override void SetTextField(string fieldValue)
        {
            App.Client.SetValueFix(LogicalName, fieldValue, ContainerType.Header);
        }

        protected override void SetTwoOptionField(BooleanValue value)
        {
            App.App.Entity.SetHeaderValue(value.ToBooleanItem(LogicalName));
        }
    }
}
