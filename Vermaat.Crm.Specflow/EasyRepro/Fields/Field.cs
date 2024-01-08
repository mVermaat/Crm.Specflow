using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using Vermaat.Crm.Specflow.EasyRepro.FieldTypes;
using Vermaat.Crm.Specflow.Entities;

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
                case AttributeTypeCode.Owner:
                    SetLookupValue(new LookupValue((EntityReference)fieldValue));
                    break;
                case AttributeTypeCode.Picklist:
                case AttributeTypeCode.Status:
                    SetOptionSetField(ToOptionSetObject(((Microsoft.Xrm.Sdk.OptionSetValue)fieldValue)?.Value, fieldValueText));
                    break;
                case AttributeTypeCode.Money:
                    SetMoneyField(new DecimalValue(((Money)fieldValue)?.Value));
                    break;
                case AttributeTypeCode.Virtual:
                    SetVirtualField(crmContext, fieldValueText);
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
                case AttributeTypeCode.PartyList:
                    SetLookupValues(ToLookupValues((EntityCollection)fieldValue).ToArray());
                    break;
                default:
                    SetTextField((string)fieldValue);
                    break;
            }

        }

        private IEnumerable<LookupValue> ToLookupValues(EntityCollection entityCollection)
        {
            var parties = Party.FromEntityCollection(entityCollection);

            foreach (var party in parties)
            {
                if (!string.IsNullOrEmpty(party.EmailAddress))
                {
                    yield return new LookupValue(new EntityReference()
                    {
                        Id = Guid.Empty,
                        LogicalName = "unresolvedaddress",
                        Name = party.EmailAddress,
                    });
                }
                else
                {
                    yield return new LookupValue(party.ConnectedParty);
                }
            }
        }

        protected virtual void SetVirtualField(CrmTestingContext crmContext, string fieldValueText)
        {
            if (Metadata.AttributeTypeName == AttributeTypeDisplayName.MultiSelectPicklistType)
            {
                if (string.IsNullOrEmpty(fieldValueText))
                {
                    SetMultiSelectOptionSetField(new MultiSelectOptionSetValue(new string[0]));
                }
                else
                {
                    SetMultiSelectOptionSetField(ToMultiSelectOptionSetObject(fieldValueText.Split(crmContext.Delimiter).Select(v => v.Trim()).ToArray()));
                }
            }
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

        protected abstract void SetLookupValues(LookupValue[] values);

        protected abstract void SetMultiSelectOptionSetField(MultiSelectOptionSetValue value);


        private FieldTypes.OptionSetValue ToOptionSetObject(int? value, string label)
        {
            if (value == null || GlobalTestingContext.LanguageCode == GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.UILanguage)
                return new FieldTypes.OptionSetValue(value, label);

            var optionMd = Metadata as EnumAttributeMetadata;
            var option = optionMd.OptionSet.Options.Where(o => o.Value == value).First();

            // Try translating to UI language, but in case there is no translation get the input label
            return new FieldTypes.OptionSetValue(value, option.Label.GetLabelInLanguage(GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.UILanguage, true) ?? label);
        }

        private MultiSelectOptionSetValue ToMultiSelectOptionSetObject(string[] labels)
        {
            if (GlobalTestingContext.LanguageCode != GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.UILanguage)
            {
                var optionMd = Metadata as MultiSelectPicklistAttributeMetadata;
                for (int i = 0; i < labels.Length; i++)
                {
                    var option = optionMd.OptionSet.Options.Where(o => o.Label.IsLabel(GlobalTestingContext.LanguageCode, labels[i])).FirstOrDefault();
                    var translation = option.Label.GetLabelInLanguage(GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.UILanguage, true);

                    if (!string.IsNullOrEmpty(translation))
                        labels[i] = translation;
                }
            }


            return new MultiSelectOptionSetValue(labels);
        }
    }
}
