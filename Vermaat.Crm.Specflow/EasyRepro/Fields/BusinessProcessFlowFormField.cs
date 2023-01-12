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
    internal class BusinessProcessFlowFormField : FormField
    {
        private readonly string _stageName;

        public BusinessProcessFlowFormField(UCIApp app, AttributeMetadata attributeMetadata, FormControl control, string stageName) 
            : base(app, attributeMetadata, control)
        {
            _stageName = stageName;
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
           SeleniumCommandProcessor.ExecuteCommand(App,
               App.SeleniumCommandFactory.CreateSetBusinessProcessFlowBooleanFieldValueCommand(value.ToBooleanItem(Metadata.LogicalName)));
        }

        protected override void SetDateTimeField(DateTimeValue value)
        {
           SeleniumCommandProcessor.ExecuteCommand(App,
               App.SeleniumCommandFactory.CreateSetBusinessProcessFlowDateTimeFieldValueCommand(LogicalName, value.Value, value.DateOnly,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.DateFormat,
                 GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeFormat));
        }

        protected override void SetOptionSetField(OptionSetValue value)
        {
            if (value.Value.HasValue)
                App.App.BusinessProcessFlow.SetValue(value.ToOptionSet(LogicalName));
            //else
                //App.App.BusinessProcessFlow.ClearValue(value.ToOptionSet(LogicalName));
        }

        protected override void SetMoneyField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected override void SetTextField(string fieldValue)
        {
            App.App.BusinessProcessFlow.SelectStage(_stageName);
            App.App.BusinessProcessFlow.SetValue(LogicalName, fieldValue);
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

        protected override void SetLookupValues(LookupValue[] values)
        {
           foreach(var value in values)
            {
                SetLookupValue(value);
            }
        }

        protected override void SetMultiSelectOptionSetField(MultiSelectOptionSetValue value)
        {
            throw new NotSupportedException("Not possible to add multiselect optionset to BPF");
        }

        public override bool IsVisible(FormState formState)
        {
            App.App.BusinessProcessFlow.SelectStage(_stageName);
            return base.IsVisible(formState);
        }
    }
}
