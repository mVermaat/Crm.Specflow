using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Vermaat.Crm.Specflow.Entities
{
    public class UserSettings
    {
        private readonly Entity _userSettingsEntity;

        public static class Fields
        {
            public const string DateFormat = "dateformatstring";
            public const string TimeFormat = "timeformatstring";
        }

        public UserSettings(Entity userSettingsEntity)
        {
            _userSettingsEntity = userSettingsEntity;
        }

        public string DateFormat => _userSettingsEntity.GetAttributeValue<string>(Fields.DateFormat);

        public string TimeFormat => _userSettingsEntity.GetAttributeValue<string>(Fields.TimeFormat);

        public string DateTimeFormat => $"{DateFormat} {TimeFormat}";
    }
}
