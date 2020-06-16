using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions
{
    public static class HelperMethods
    {
        public static string GetAppSettingsValue(string key, bool emptyAllowed = false, string defaultValue = null)
        {
            string value = ConfigurationManager.AppSettings[key];

            if (!emptyAllowed && string.IsNullOrEmpty(value))
                throw new TestExecutionException(Constants.ErrorCodes.APP_SETTINGS_REQUIRED, key);

            return value ?? defaultValue;
        }

        internal static bool IsLabel(this Label label, int lcid, string name)
        {
            return name.Equals(label.GetLabelInLanguage(lcid), StringComparison.CurrentCultureIgnoreCase);
        }

        internal static string GetLabelInLanguage(this Label label, int lcid)
        {
            string result = label.LocalizedLabels.Where(l => l.LanguageCode == lcid).FirstOrDefault()?.Label;

            if (label.UserLocalizedLabel != null && !string.IsNullOrEmpty(label.UserLocalizedLabel.Label) && string.IsNullOrEmpty(result))
                throw new TestExecutionException(Constants.ErrorCodes.LABEL_NOT_TRANSLATED, label.UserLocalizedLabel.Label, lcid);

            return result;
        }

        internal static EntityReference ToEntityReference(this Entity entity, string primaryFieldAttribute)
        {
            return new EntityReference(entity.LogicalName, entity.Id) { Name = entity.GetAttributeValue<string>(primaryFieldAttribute) };
        }
    }
}
