using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro.FieldTypes;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    public abstract class Field
    {
        protected AttributeMetadata Metadata { get; }
        protected UCIApp App { get; }

        protected virtual string LogicalName => Metadata.LogicalName;

        public Field(UCIApp app, AttributeMetadata metadata)
        {
            Metadata = metadata;
            App = app;
        }

        public virtual void SetValue(CrmTestingContext crmContext, string fieldValueText)
        {
            object fieldValue = ObjectConverter.ToCrmObject(Metadata.EntityLogicalName, Metadata.LogicalName, fieldValueText, crmContext);

            if (fieldValue != null)
            {
                Logger.WriteLine($"Setting field value");
                switch (Metadata.AttributeType.Value)
                {
                    case AttributeTypeCode.Boolean:
                        SetTwoOptionField(new BooleanValue((bool)fieldValue));
                        break;
                    case AttributeTypeCode.DateTime:
                        SetDateTimeField(new DateTimeValue((DateTimeAttributeMetadata)Metadata, (DateTime)fieldValue));
                        break;
                    case AttributeTypeCode.Customer:
                    case AttributeTypeCode.Lookup:
                        SetLookupValue(new LookupValue((EntityReference)fieldValue));
                        break;
                    case AttributeTypeCode.Picklist:
                        SetOptionSetField(new FieldTypes.OptionSetValue(((Microsoft.Xrm.Sdk.OptionSetValue)fieldValue).Value, fieldValueText));
                        break;
                    case AttributeTypeCode.Money:
                        SetMoneyField(new DecimalValue(((Money)fieldValue).Value));
                        break;
                    case AttributeTypeCode.Virtual:
                        SetVirtualField(fieldValueText);
                        break;
                    case AttributeTypeCode.Integer:
                        SetIntegerField(new IntegerValue((int)fieldValue));
                        break;
                    case AttributeTypeCode.Double:
                        SetDoubleField(new DoubleValue((double)fieldValue));
                        break;
                    case AttributeTypeCode.BigInt:
                        SetLongField(new LongValue((long)fieldValue));
                        break;
                    case AttributeTypeCode.Decimal:
                        SetDecimalField(new DecimalValue((decimal)fieldValue));
                        break;
                    default:
                        SetTextField((string)fieldValue);
                        break;
                }
            }
            else
            {
                Logger.WriteLine($"Clearing field value");
                ClearValue();
            }
        }

        protected virtual void ClearValue()
        {
            switch (Metadata.AttributeType.Value)
            {
                case AttributeTypeCode.Boolean:
                    throw new TestExecutionException(Constants.ErrorCodes.TWO_OPTION_FIELDS_CANT_BE_CLEARED);
                case AttributeTypeCode.Customer:
                case AttributeTypeCode.Lookup:
                    App.App.Entity.ClearValue(new LookupItem { Name = LogicalName });
                    break;
                case AttributeTypeCode.Picklist:
                    App.App.Entity.ClearValue(new OptionSet { Name = LogicalName });
                    break;
                case AttributeTypeCode.DateTime:
                    App.Client.SetValueFix(LogicalName, null, null, null);
                    break;
                default:
                    SetTextField(null);
                    break;
            }
        }


        protected virtual void SetIntegerField(IntegerValue value)
        {
            SetTextField(value.TextValue);
        }

        protected virtual void SetDoubleField(DoubleValue value)
        {
            SetTextField(value.TextValue);
        }

        protected virtual void SetDecimalField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected virtual void SetLongField(LongValue value)
        {
            SetTextField(value.TextValue);
        }

        protected virtual void SetTwoOptionField(BooleanValue value)
        {
            App.App.Entity.SetValue(value.ToBooleanItem(Metadata.LogicalName));
        }

        protected virtual void SetDateTimeField(DateTimeValue value)
        {
            App.Client.SetValueFix(LogicalName, value.Value, 
                GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.DateFormat, 
                GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeFormat);
        }

        protected virtual void SetOptionSetField(FieldTypes.OptionSetValue value)
        {
            App.App.Entity.SetValue(value.ToOptionSet(Metadata));
        }

        protected virtual void SetMoneyField(DecimalValue value)
        {
            SetTextField(value.TextValue);
        }

        protected virtual void SetTextField(string fieldValue)
        {
            App.Client.SetValueFix(LogicalName, fieldValue, FormContextType.Entity);
        }

        protected virtual void SetLookupValue(LookupValue value)
        {
            App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').setValue([ {{ id: '{value.Value.Id}', name: '{value.Value.Name.Replace("'", @"\'")}', entityType: '{value.Value.LogicalName}' }} ])");
            App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').fireOnChange()");
        }


        protected virtual void SetVirtualField(string fieldValueText)
        {
            if (Metadata.AttributeTypeName == AttributeTypeDisplayName.MultiSelectPicklistType)
                App.App.Entity.SetValue(new MultiValueOptionSet { Name = LogicalName, Values = fieldValueText.Split(',').Select(v => v.Trim()).ToArray() });
            else
                throw new NotImplementedException(string.Format("Virtual type {0} not implemented", Metadata.AttributeTypeName.Value));
        }

       
        

        

        

        

       
    }
}
