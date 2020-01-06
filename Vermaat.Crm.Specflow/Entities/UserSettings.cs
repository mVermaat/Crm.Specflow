using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using OpenQA.Selenium;

namespace Vermaat.Crm.Specflow.Entities
{
    public class UserSettings
    {
        private readonly Entity _userSettingsEntity;

        public static class Fields
        {
            public const string DateFormat = "dateformatstring";
            public const string TimeFormat = "timeformatstring";
            public const string DateSeparator = "dateseparator";
            public const string TimeSeparator = "timeseparator";
        }

        private UserSettings(Entity userSettingsEntity, TimeZoneInfo timeZoneInfo)
        {
            _userSettingsEntity = userSettingsEntity;
            TimeZoneInfo = timeZoneInfo;
        }

        /// <summary>
        /// Gets setting of the specified user.
        /// </summary>
        /// <param name="service">The service client.</param>
        /// <param name="userName">The user name.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when user can't be found.</exception>
        public static UserSettings GetUserSettings(CrmService service, string userName)
        {
            var userQuery = new QueryExpression("systemuser")
            {
                ColumnSet = { AllColumns = true }, TopCount = 1
            };
            userQuery.Criteria.AddCondition("domainname", ConditionOperator.Equal, userName);
            var userEntity = service.RetrieveMultiple(userQuery);

            if (!userEntity.Entities.Any())
            {
                throw new NotFoundException($"The user with domainname '{userName}' is not found!");
            }

            var userGuid = userEntity.Entities[0]["systemuserid"];
            var query = new QueryExpression("usersettings")
            {
                TopCount = 1,
                ColumnSet = { AllColumns = true }
            };
            query.Criteria.AddCondition("systemuserid", ConditionOperator.Equal, userGuid);
            var settingsEntity = service.RetrieveMultiple(query).Entities[0];

            query = new QueryExpression("timezonedefinition")
            {
                TopCount = 1
            };
            query.ColumnSet.AddColumn("standardname");
            query.Criteria.AddCondition("timezonecode", ConditionOperator.Equal, settingsEntity["timezonecode"]);
            var timeZoneEntity = service.RetrieveMultiple(query).Entities[0];
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneEntity.GetAttributeValue<string>("standardname"));

            return new UserSettings(settingsEntity, timeZoneInfo);
        }

        public string DateFormat => _userSettingsEntity.GetAttributeValue<string>(Fields.DateFormat)
            .Replace("/", _userSettingsEntity.GetAttributeValue<string>(Fields.DateSeparator));

        public string TimeFormat => _userSettingsEntity.GetAttributeValue<string>(Fields.TimeFormat)
            .Replace(":", _userSettingsEntity.GetAttributeValue<string>(Fields.TimeSeparator));

        public string DateTimeFormat => $"{DateFormat} {TimeFormat}";

        public TimeZoneInfo TimeZoneInfo { get; }
    }
}
