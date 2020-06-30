using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerPlatform.SpecflowExtensions.EasyRepro.FieldTypes;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Controls
{
    internal abstract class Field : Control
    {
        protected AttributeMetadata Metadata { get; }

        protected virtual string LogicalName => Metadata.LogicalName;

        public Field(AttributeMetadata metadata, string control)
            : base(control)
        {
            Metadata = metadata;
        }

        public virtual void SetValue(ICrmContext crmContext, string fieldValueText)
        {
            object fieldValue = ObjectConverter.ToCrmObject(Metadata.EntityLogicalName, Metadata.LogicalName, fieldValueText, crmContext);

            Logger.WriteLine($"Setting field value");
            switch (Metadata.AttributeType.Value)
            {
                case AttributeTypeCode.Boolean:
                    SetTwoOptionField(new BooleanValue((bool?)fieldValue));
                    break;
                case AttributeTypeCode.DateTime:
                    SetDateTimeField(new DateTimeValue((DateTimeAttributeMetadata)Metadata, (DateTime?)fieldValue));
                    break;
                case AttributeTypeCode.Customer:
                case AttributeTypeCode.Lookup:
                    SetLookupValue(new LookupValue((EntityReference)fieldValue));
                    break;
                case AttributeTypeCode.Picklist:
                    SetOptionSetField(new FieldTypes.OptionSetValue(((Microsoft.Xrm.Sdk.OptionSetValue)fieldValue)?.Value, fieldValueText));
                    break;
                case AttributeTypeCode.Money:
                    SetMoneyField(new DecimalValue(((Money)fieldValue)?.Value));
                    break;
                case AttributeTypeCode.Virtual:
                    SetVirtualField(fieldValueText);
                    break;
                case AttributeTypeCode.Integer:
                    SetIntegerField(new IntegerValue((int?)fieldValue));
                    break;
                case AttributeTypeCode.Double:
                    SetDoubleField(new DoubleValue((double?)fieldValue));
                    break;
                case AttributeTypeCode.BigInt:
                    SetLongField(new LongValue((long?)fieldValue));
                    break;
                case AttributeTypeCode.Decimal:
                    SetDecimalField(new DecimalValue((decimal?)fieldValue));
                    break;
                default:
                    SetTextField((string)fieldValue);
                    break;
            }

        }

        protected virtual void SetVirtualField(string fieldValueText)
        {
            if (Metadata.AttributeTypeName == AttributeTypeDisplayName.MultiSelectPicklistType)
                SetMultiSelectOptionSetField(new MultiSelectOptionSetValue(fieldValueText.Split(',').Select(v => v.Trim()).ToArray()));
            else
                throw new NotImplementedException(string.Format("Virtual type {0} not implemented", Metadata.AttributeTypeName.Value));
        }


        protected abstract void SetIntegerField(IntegerValue value);

        protected abstract void SetDoubleField(DoubleValue value);

        protected abstract void SetDecimalField(DecimalValue value);

        protected abstract void SetLongField(LongValue value);

        protected abstract void SetTwoOptionField(BooleanValue value);

        protected abstract void SetDateTimeField(DateTimeValue value);

        protected abstract void SetOptionSetField(FieldTypes.OptionSetValue value);

        protected abstract void SetMoneyField(DecimalValue value);

        protected abstract void SetTextField(string fieldValue);

        protected abstract void SetLookupValue(LookupValue value);

        protected abstract void SetMultiSelectOptionSetField(MultiSelectOptionSetValue value);

    }
}
