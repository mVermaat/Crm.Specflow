using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
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

        public void SetValue(CrmTestingContext crmContext, string fieldValueText)
        {
            object fieldValue = ObjectConverter.ToCrmObject(Metadata.EntityLogicalName, Metadata.LogicalName, fieldValueText, crmContext);

            if (fieldValue != null)
            {
                Logger.WriteLine($"Setting field value");
                switch (Metadata.AttributeType.Value)
                {
                    case AttributeTypeCode.Boolean:
                        SetTwoOptionField((bool)fieldValue, fieldValueText);
                        break;
                    case AttributeTypeCode.DateTime:
                        SetDateTimeField((DateTime)fieldValue, fieldValueText);
                        break;
                    case AttributeTypeCode.Customer:
                    case AttributeTypeCode.Lookup:
                        SetLookupValue((EntityReference)fieldValue, fieldValueText);
                        break;
                    case AttributeTypeCode.Picklist:
                        SetOptionSetField((OptionSetValue)fieldValue, fieldValueText);
                        break;
                    case AttributeTypeCode.Money:
                        SetMoneyField((Money)fieldValue, fieldValueText);
                        break;
                    case AttributeTypeCode.Virtual:
                        SetVirtualField(fieldValueText);
                        break;
                    case AttributeTypeCode.Integer:
                    case AttributeTypeCode.Double:
                    case AttributeTypeCode.BigInt:
                    case AttributeTypeCode.Decimal:
                        SetTextField(fieldValueText);
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

        protected virtual void SetVirtualField(string fieldValueText)
        {
            if (Metadata.AttributeTypeName == AttributeTypeDisplayName.MultiSelectPicklistType)
                App.App.Entity.SetValue(new MultiValueOptionSet { Name = LogicalName, Values = fieldValueText.Split(',').Select(v => v.Trim()).ToArray() });
            else
                throw new NotImplementedException(string.Format("Virtual type {0} not implemented", Metadata.AttributeTypeName.Value));
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
                default:
                    SetTextField(null);
                    break;
            }
        }

        protected virtual void SetTwoOptionField(bool fieldValueBool, string fieldValueText)
        {
            App.App.Entity.SetValue(new BooleanItem { Name = LogicalName, Value = fieldValueBool });
        }

        protected virtual void SetDateTimeField(DateTime fieldValue, string fieldValueText)
        {
            string format = ((DateTimeAttributeMetadata)Metadata).Format == DateTimeFormat.DateAndTime
                ? GlobalTestingContext.ConnectionManager.CurrentUserDetails.UserSettings.DateTimeFormat
                : GlobalTestingContext.ConnectionManager.CurrentUserDetails.UserSettings.DateFormat;

            if (((DateTimeAttributeMetadata)Metadata).DateTimeBehavior == DateTimeBehavior.UserLocal)
            {
                var offset = GlobalTestingContext.ConnectionManager.CurrentUserDetails.UserSettings.TimeZoneInfo.GetUtcOffset(fieldValue);
                App.Client.SetValueFix(LogicalName, fieldValue.Add(offset), format);
            }
            else
            {
                App.Client.SetValueFix(LogicalName, fieldValue, format);
            }
        }

        protected virtual void SetOptionSetField(OptionSetValue optionSetNumber, string optionSetLabel)
        {
            App.App.Entity.SetValue(new OptionSet { Name = LogicalName, Value = optionSetLabel });
        }

        protected virtual void SetMoneyField(Money fieldValue, string fieldValueText)
        {
            SetTextField(fieldValue?.Value.ToString());
        }

        protected virtual void SetTextField(string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                App.App.Entity.ClearValue(LogicalName);
            else
                App.Client.SetValueFix(LogicalName, fieldValue);
        }

        protected virtual void SetLookupValue(EntityReference fieldValue, string fieldValueText)
        {
            App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').setValue([ {{ id: '{fieldValue.Id}', name: '{fieldValue.Name.Replace("'", @"\'")}', entityType: '{fieldValue.LogicalName}' }} ])");
            App.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{LogicalName}').fireOnChange()");
        }
    }
}
