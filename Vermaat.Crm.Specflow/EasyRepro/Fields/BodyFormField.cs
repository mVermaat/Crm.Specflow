using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro.FieldTypes;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    public class BodyFormField : FormField
    {
        private string _tabLabel;

        public BodyFormField(UCIApp app, AttributeMetadata attributeMetadata, string control) 
            : base(app, attributeMetadata, control)
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
            if (string.IsNullOrEmpty(_tabLabel))
            {
                _tabLabel = App.WebDriver.ExecuteScript($"return Xrm.Page.getControl('{Control}').getParent().getParent().getLabel()")?.ToString();
            }
            return _tabLabel;
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
            App.Client.SetValueFix(LogicalName, value.Value,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.DateFormat,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeFormat);
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
                App.App.Entity.ClearValue(value.ToLookupItem(Metadata));
            }
        }
    }
}
