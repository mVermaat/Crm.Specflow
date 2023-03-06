using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow
{
    public static class ObjectConverter
    {
        private static readonly string _datetimeFormat;
        private static readonly string _dateonlyFormat;


        static ObjectConverter()
        {
            _dateonlyFormat = HelperMethods.GetAppSettingsValue("DateFormat", false);
            _datetimeFormat = $"{_dateonlyFormat} {HelperMethods.GetAppSettingsValue("TimeFormat", false)}";
        }

        public static object ToCrmObject(string entityName, string attributeName, string value, CrmTestingContext context, ConvertedObjectType objectType = ConvertedObjectType.Default)
        {
            Logger.WriteLine($"Converting CRM Object. Entity: {entityName}, Attribute: {attributeName}, Value: {value}, ObjectType: {objectType}");
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var metadata = GlobalTestingContext.Metadata.GetAttributeMetadata(entityName, attributeName);
            object convertedValue = GetConvertedValue(context, metadata, value, objectType);
            Logger.WriteLine($"ConvertedValue: {HelperMethods.CrmObjectToPrimitive(convertedValue)}");

            return convertedValue;
        }

        public static string ToSpecflowTableValue(AttributeMetadata metadata, object parsed)
        {
            switch (metadata.AttributeType)
            {
                case AttributeTypeCode.Double:
                    return Convert.ToDouble(parsed).ToString(NumberFormatInfo.InvariantInfo);

                case AttributeTypeCode.Decimal:
                case AttributeTypeCode.Money:
                    return Convert.ToDecimal(parsed).ToString(NumberFormatInfo.InvariantInfo);

                case AttributeTypeCode.Integer:
                    return Convert.ToInt32(parsed).ToString(NumberFormatInfo.InvariantInfo);

                case AttributeTypeCode.DateTime:
                    return ToDateTimeString(metadata, (DateTime)parsed);

                default:
                    return parsed.ToString();
            }
        }

        private static string ToDateTimeString(AttributeMetadata metadata, DateTime parsed)
        {
            var format = ((DateTimeAttributeMetadata)metadata).Format == DateTimeFormat.DateOnly ?
                _dateonlyFormat : _datetimeFormat;

            return parsed.ToString(format);
        }

        private static object GetConvertedValue(CrmTestingContext context, AttributeMetadata metadata, string value, ConvertedObjectType objectType)
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

                case AttributeTypeCode.EntityName:
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
                    return Guid.Parse(value);

                case AttributeTypeCode.Virtual:
                    return ParseVirtualType(context, metadata, value);

                case AttributeTypeCode.PartyList:
                    return ParsePartyList(context, metadata, value);

                //case AttributeTypeCode.EntityName:
                //    return ParseEntityName(context, metadata, value);

                default: throw new NotImplementedException(string.Format("Type {0} not implemented", metadata.AttributeType));
            }
        }

        private static object ParseEntityName(CrmTestingContext context, AttributeMetadata metadata, string value)
        {
            var data = GlobalTestingContext.ConnectionManager.CurrentConnection.RetrieveMultiple(new QueryExpression(metadata.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true),
                TopCount = 100,
                
            });

            throw new NotImplementedException();
        }

        private static object ParsePartyList(CrmTestingContext context, AttributeMetadata metadata, string value)
        {
            var splitted = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            EntityCollection partyList = new EntityCollection();
            partyList.EntityName = "activityparty";
            foreach (var item in splitted)
            {
                var entityRef = GetLookupValue(context, metadata, item.Trim());
                partyList.Entities.Add(ToActivityParty(entityRef, metadata));
            }

            return partyList;

        }

        private static Entity ToActivityParty(EntityReference entityRef, AttributeMetadata metadata)
        {
            var party = new Entity("activityparty", entityRef.Id);

            party["participationtypemask"] = GetTypeMask(metadata);
            party["partyid"] = entityRef;

            return party;
        }

        private static OptionSetValue GetTypeMask(AttributeMetadata metadata)
        {
            switch (metadata.LogicalName)
            {
                case "from": return new OptionSetValue(1);
                case "to": return new OptionSetValue(2);
                case "cc": return new OptionSetValue(3);
                case "bcc": return new OptionSetValue(4);
                case "requiredattendees": return new OptionSetValue(5);
                case "optionalattendees": return new OptionSetValue(6);
                case "organizer": return new OptionSetValue(7);

                default: throw new NotImplementedException($"Type mask for field {metadata.LogicalName} not implemented");
            }
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
                    GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.TimeZoneInfo);
            }
            else
            {
                return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
        }

        private static object ParseVirtualType(CrmTestingContext context, AttributeMetadata metadata, string value)
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

        public static SetStateRequest ToSetStateRequest(EntityReference target, string desiredstatus, CrmTestingContext context)
        {
            var attributeMd = GlobalTestingContext.Metadata.GetAttributeMetadata(target.LogicalName, Constants.CRM.STATUSCODE) as StatusAttributeMetadata;
            var optionMd = attributeMd.OptionSet.Options.Where(o => o.Label.IsLabel(GlobalTestingContext.LanguageCode, desiredstatus)).FirstOrDefault() as StatusOptionMetadata;

            return new SetStateRequest()
            {
                EntityMoniker = target,
                State = new OptionSetValue(optionMd.State.Value),
                Status = new OptionSetValue(optionMd.Value.Value),
            };
        }

        public static EntityReference GetLookupValue(CrmTestingContext context, string alias, string targetEntity)
        {
            return GetLookupValue(context, alias, targetEntity, new ConditionExpression[0]);
        }

        public static EntityReference GetLookupValue(CrmTestingContext context, string alias, string targetEntity, IEnumerable<ConditionExpression> addtionalLookupFilters)
        {
            Logger.WriteLine($"Getting lookupvalue for entity {targetEntity}");

            var result = context.RecordCache.Get(alias);
            if (result != null)
            {
                Logger.WriteLine($"Cached record found");
                return result;
            }

            var targetMd = GlobalTestingContext.Metadata.GetEntityMetadata(targetEntity);

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

            var col = GlobalTestingContext.ConnectionManager.CurrentConnection.RetrieveMultiple(qe);

            Logger.WriteLine($"Looked for {targetEntity} with {targetMd.PrimaryNameAttribute} is {alias}. Found {col.Entities.Count} records");
            return col.Entities.FirstOrDefault()?.ToEntityReference(targetMd.PrimaryNameAttribute);
        }

        private static EntityReference GetLookupValue(CrmTestingContext context, AttributeMetadata metadata, string alias)
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

        private static OptionSetValue GetOptionSetValue(AttributeMetadata metadata, string value, CrmTestingContext context)
        {
            var optionMd = metadata as EnumAttributeMetadata;

            var option = optionMd.OptionSet.Options.Where(o => o.Label.IsLabel(GlobalTestingContext.LanguageCode, value)).FirstOrDefault();

            Assert.IsNotNull(option, $"Option {value} not found. AvailaleOptions: {string.Join(", ", optionMd.OptionSet.Options.Select(o => o.Label?.GetLabelInLanguage(GlobalTestingContext.LanguageCode)))}");
            Assert.IsTrue(option.Value.HasValue);

            return new OptionSetValue(option.Value.Value);
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
        private static object GetTwoOptionValue(AttributeMetadata metadata, string value, CrmTestingContext context)
        {
            var optionMd = metadata as BooleanAttributeMetadata;

            if (value.ToLower() == optionMd.OptionSet.TrueOption.Label.GetLabelInLanguage(GlobalTestingContext.LanguageCode).ToLower())
                return true;
            else if (value.ToLower() == optionMd.OptionSet.FalseOption.Label.GetLabelInLanguage(GlobalTestingContext.LanguageCode).ToLower())
                return false;
            else
                throw new TestExecutionException(Constants.ErrorCodes.OPTION_NOT_FOUND, metadata.LogicalName, value);
        }
    }
}
