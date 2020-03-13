using System;
using System.Collections.Generic;
using System.Globalization;
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
            public const string DecimalSymbol = "decimalsymbol";
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
