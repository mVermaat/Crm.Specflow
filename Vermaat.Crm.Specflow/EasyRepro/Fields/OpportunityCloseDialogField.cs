using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using Vermaat.Crm.Specflow.EasyRepro.FieldTypes;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    class OpportunityCloseDialogField : Field
    {
        protected override string LogicalName => GetLogicalName();

        public OpportunityCloseDialogField(UCIApp app, AttributeMetadata metadata)
            : base(app, metadata)
        {
        }

        private string GetLogicalName()
        {
            if (Metadata.LogicalName == "opportunitystatuscode")
                return "statusreason_id";
            else
                return base.LogicalName + "_id";
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
            App.App.Entity.SetValue(value.ToBooleanItem(LogicalName));
        }

        protected override void SetDateTimeField(DateTimeValue value)
        {
            App.Client.SetValueFix(LogicalName, value.Value, value.DateOnly,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.DateFormat,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeFormat);
        }

        protected override void SetOptionSetField(FieldTypes.OptionSetValue value)
        {
            if (value.Value.HasValue)
                App.Client.SetValueFix(value.ToOptionSet(LogicalName), ContainerType.Dialog);
            else
                App.App.Entity.ClearValue(value.ToOptionSet(LogicalName));
        }

        protected override void SetMoneyField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetTextField(string fieldValue)
        {
            App.Client.SetValueFix(LogicalName, fieldValue, ContainerType.Dialog);
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
        
    }
}
