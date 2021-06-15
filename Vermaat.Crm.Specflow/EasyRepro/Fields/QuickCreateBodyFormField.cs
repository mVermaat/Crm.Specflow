using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using Vermaat.Crm.Specflow.EasyRepro.FieldTypes;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    public class QuickCreateBodyFormField : FormField
    {
        public QuickCreateBodyFormField(UCIApp app, AttributeMetadata attributeMetadata, string control)
            : base(app, attributeMetadata, control)
        {
        }

        protected override void SetMultiSelectOptionSetField(MultiSelectOptionSetValue value)
        {
            App.App.QuickCreate.SetValue(value.ToMultiValueOptionSet(LogicalName), true);
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
            App.App.QuickCreate.SetValue(value.ToBooleanItem(Metadata.LogicalName));
        }

        protected override void SetDateTimeField(DateTimeValue value)
        {
            App.Client.SetValueFix(LogicalName, value.Value, value.DateOnly,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.DateFormat,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeFormat);
        }

        protected override void SetOptionSetField(OptionSetValue value)
        {
            if (value.Value.HasValue)
                App.App.QuickCreate.SetValue(value.ToOptionSet(LogicalName));
            else
                App.App.QuickCreate.ClearValue(value.ToOptionSet(LogicalName));
        }

        protected override void SetMoneyField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetTextField(string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                App.App.QuickCreate.ClearValue(LogicalName);
            else
                App.App.QuickCreate.SetValue(LogicalName, fieldValue);
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
                App.App.QuickCreate.ClearValue(value.ToLookupItem(Metadata));
            }
        }
    }
}
