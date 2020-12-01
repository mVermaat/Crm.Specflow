using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.Interfaces;
using PowerPlatform.SpecflowExtensions.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions
{
    public static class ObjectConverter
    {
        private static readonly string _datetimeFormat;
        private static readonly string _dateonlyFormat;


        static ObjectConverter()
        {
            _dateonlyFormat = HelperMethods.GetAppSettingsValue(Constants.AppSettings.DATE_FORMAT , false);
            _datetimeFormat = $"{_dateonlyFormat} {HelperMethods.GetAppSettingsValue(Constants.AppSettings.TIME_FORMAT, false)}";
        }

        public static object ToCrmObject(string entityName, string attributeName, string value, ICrmContext context, ConvertedObjectType objectType = ConvertedObjectType.Default)
        {
            Logger.WriteLine($"Converting CRM Object. Entity: {entityName}, Attribute: {attributeName}, Value: {value}, ObjectType: {objectType}");
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var metadata = GlobalContext.Metadata.GetAttributeMetadata(entityName, attributeName);
            object convertedValue = GetConvertedValue(context, metadata, value, objectType);
            Logger.WriteLine($"ConvertedValue: {CrmObjectToPrimitive(convertedValue)}");

            return convertedValue;
        }

        public static EntityReference GetLookupValue(ICrmContext context, string textValue, string targetEntity)
        {
            return GetLookupValue(context, textValue, targetEntity, new ConditionExpression[0]);
        }

        public static Status FromStatusText(EntityReference target, string desiredstatus, ICrmContext context)
        {
            var attributeMd = GlobalContext.Metadata.GetAttributeMetadata(target.LogicalName, "statuscode") as StatusAttributeMetadata;
            var optionMd = attributeMd.OptionSet.Options.Where(o => o.Label.IsLabel(context.LanguageCode, desiredstatus)).FirstOrDefault() as StatusOptionMetadata;

            return new Status
            {
                StateCode = optionMd.State.Value,
                StatusCode = optionMd.Value.Value,
            };
        }

        private static object CrmObjectToPrimitive(object value)
        {
            if (value == null)
                return null;

            Type type = value.GetType();
            if (type == typeof(OptionSetValue))
            {
                return ((OptionSetValue)value).Value;
            }
            else if (type == typeof(EntityReference))
            {
                return ((EntityReference)value).Id;
            }
            else if (type == typeof(Money))
            {
                return ((Money)value).Value;
            }
            else if (type == typeof(OptionSetValueCollection))
            {
                return ((OptionSetValueCollection)value).Where(ov => ov != null).Select(ov => ov.Value);
            }
            return value;
        }

        private static object GetConvertedValue(ICrmContext context, AttributeMetadata metadata, string value, ConvertedObjectType objectType)
        {
            switch (metadata.AttributeType)
            {
                case AttributeTypeCode.Boolean:
                    return GetTwoOptionValue(metadata, value, context);
                case AttributeTypeCode.Double: return double.Parse(value, NumberFormatInfo.InvariantInfo);
                case AttributeTypeCode.Decimal: return decimal.Parse(value, NumberFormatInfo.InvariantInfo);
                case AttributeTypeCode.Integer: return int.Parse(value, NumberFormatInfo.InvariantInfo);
                case AttributeTypeCode.DateTime:
                    return ParseDateTime(metadata, value);

                case AttributeTypeCode.Memo:
                case AttributeTypeCode.String: return value;

                case AttributeTypeCode.Money:
                    if (objectType == ConvertedObjectType.Primitive)
                        return decimal.Parse(value, NumberFormatInfo.InvariantInfo);
                    else
                        return new Money(decimal.Parse(value, NumberFormatInfo.InvariantInfo));
                case AttributeTypeCode.Picklist:
                case AttributeTypeCode.State:
                case AttributeTypeCode.Status:
                    var optionSet = GetOptionSetValue(metadata, value, context);
                    if (objectType == ConvertedObjectType.Primitive)
                        return optionSet.Value;
                    else
                        return optionSet;

                case AttributeTypeCode.Customer:
                case AttributeTypeCode.Lookup:
                case AttributeTypeCode.Owner:
                    var lookup = GetLookupValue(context, metadata, value);
                    if (objectType == ConvertedObjectType.Primitive)
                        return lookup.Id;
                    else
                        return lookup;

                case AttributeTypeCode.Uniqueidentifier:
                    return GetLookupValue(context, metadata, value).Id;

                case AttributeTypeCode.Virtual:
                    return ParseVirtualType(context, metadata, value);

                default: throw new NotImplementedException(string.Format("Type {0} not implemented", metadata.AttributeType));
            }
        }

        /// <summary>
        /// Gets the TwoOptionValue. 
        /// 
        /// For UI type it will return the same text, but just verifies if the text is a valid option
        /// For other types it will return a boolean that matches the text
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private static object GetTwoOptionValue(AttributeMetadata metadata, string value, ICrmContext context)
        {
            var optionMd = metadata as BooleanAttributeMetadata;

            if (value.ToLower() == optionMd.OptionSet.TrueOption.Label.GetLabelInLanguage(context.LanguageCode).ToLower())
                return true;
            else if (value.ToLower() == optionMd.OptionSet.FalseOption.Label.GetLabelInLanguage(context.LanguageCode).ToLower())
                return false;
            else
                throw new TestExecutionException(Constants.ErrorCodes.OPTION_NOT_FOUND, metadata.LogicalName, value);
        }

        private static DateTime? ParseDateTime(AttributeMetadata metadata, string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var format = ((DateTimeAttributeMetadata)metadata).Format == DateTimeFormat.DateOnly ?
                _dateonlyFormat : _datetimeFormat;

            if (((DateTimeAttributeMetadata)metadata).DateTimeBehavior == DateTimeBehavior.UserLocal)
            {
                return TimeZoneInfo.ConvertTimeToUtc(
                    DateTime.ParseExact(value, format, CultureInfo.InvariantCulture),
                    GlobalContext.ConnectionManager.CurrentCrmService.UserSettings.TimeZoneInfo);
            }
            else
            {
                return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
        }

        private static OptionSetValue GetOptionSetValue(AttributeMetadata metadata, string value, ICrmContext context)
        {
            var optionMd = metadata as EnumAttributeMetadata;

            var option = optionMd.OptionSet.Options.Where(o => o.Label.IsLabel(context.LanguageCode, value)).FirstOrDefault();

            option.Should().NotBeNull($"Option {value} not found. AvailaleOptions: { string.Join(", ", optionMd.OptionSet.Options.Select(o => o.Label?.GetLabelInLanguage(context.LanguageCode)))}");
            option.Value.Should().HaveValue();

            return new OptionSetValue(option.Value.Value);
        }


        private static EntityReference GetLookupValue(ICrmContext context, string alias, string targetEntity, IEnumerable<ConditionExpression> addtionalLookupFilters)
        {
            Logger.WriteLine($"Getting lookupvalue for entity {targetEntity}");

            var result = context.RecordCache.Get(alias);
            if (result != null)
            {
                Logger.WriteLine($"Cached record found");
                return result;
            }

            var targetMd = GlobalContext.Metadata.GetEntityMetadata(targetEntity);

            Logger.WriteLine($"Querying lookup in CRM");
            QueryExpression qe = new QueryExpression(targetEntity)
            {
                ColumnSet = new ColumnSet(targetMd.PrimaryNameAttribute)
            };
            qe.Criteria.AddCondition(targetMd.PrimaryNameAttribute, ConditionOperator.Equal, alias);

            if (addtionalLookupFilters != null)
            {
                foreach (var filter in addtionalLookupFilters)
                {
                    qe.Criteria.AddCondition(filter);
                }
            }

            var col = GlobalContext.ConnectionManager.CurrentCrmService.RetrieveMultiple(qe);

            Logger.WriteLine($"Looked for {targetEntity} with {targetMd.PrimaryNameAttribute} is {alias}. Found {col.Entities.Count} records");
            return col.Entities.FirstOrDefault()?.ToEntityReference(targetMd.PrimaryNameAttribute);
        }

        private static EntityReference GetLookupValue(ICrmContext context, AttributeMetadata metadata, string alias)
        {
            var lookupMd = (LookupAttributeMetadata)metadata;
            foreach (string targetEntity in lookupMd.Targets)
            {
                var result = GetLookupValue(context, alias, targetEntity);
                if (result != null)
                {
                    return result;
                }
            }

            throw new TestExecutionException(Constants.ErrorCodes.LOOKUP_NOT_FOUND, alias, string.Join(", ", lookupMd.Targets));
        }

        private static object ParseVirtualType(ICrmContext context, AttributeMetadata metadata, string value)
        {

            if (metadata.AttributeTypeName == AttributeTypeDisplayName.MultiSelectPicklistType)
            {
                return new OptionSetValueCollection(value.Split(',').Select(v => GetOptionSetValue(metadata, v.Trim(), context)).ToList());
            }
            else
            {
                throw new NotImplementedException(string.Format("Virtual type {0} not implemented", metadata.AttributeTypeName.Value));
            }
        }
    }
}
