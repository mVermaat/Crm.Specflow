using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace PowerPlatform.SpecflowExtensions.Models
{
    public class UserSettings
    {
        internal const string EntityLogicalName = "usersettings";

        private readonly Entity _userSettingsEntity;

        internal static class Fields
        {
            internal const string DateFormat = "dateformatstring";
            internal const string TimeFormat = "timeformatstring";
            internal const string DateSeparator = "dateseparator";
            internal const string TimeSeparator = "timeseparator";
            internal const string DecimalSymbol = "decimalsymbol";
            internal const string UserId = "systemuserid";
            internal const string TimeZoneCode = "timezonecode";
        }

        public UserSettings(Entity userSettingsEntity, TimeZoneInfo timeZoneInfo)
        {
            _userSettingsEntity = userSettingsEntity;
            TimeZoneInfo = timeZoneInfo;
            NumberFormat = GetNumberFormatInfo();

        }

        private NumberFormatInfo GetNumberFormatInfo()
        {
            var nfi = (NumberFormatInfo)NumberFormatInfo.InvariantInfo.Clone();
            nfi.NumberDecimalSeparator = _userSettingsEntity.GetAttributeValue<string>(Fields.DecimalSymbol);
            return nfi;
        }

        public string DateFormat => _userSettingsEntity.GetAttributeValue<string>(Fields.DateFormat)
            .Replace("/", _userSettingsEntity.GetAttributeValue<string>(Fields.DateSeparator));

        public string TimeFormat => _userSettingsEntity.GetAttributeValue<string>(Fields.TimeFormat)
            .Replace(":", _userSettingsEntity.GetAttributeValue<string>(Fields.TimeSeparator));

        public string DateTimeFormat => $"{DateFormat} {TimeFormat}";

        public NumberFormatInfo NumberFormat { get; }

        public TimeZoneInfo TimeZoneInfo { get; }

        public Guid Id => _userSettingsEntity.Id;
    }
}
