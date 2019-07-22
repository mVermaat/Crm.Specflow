using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Vermaat.Crm.Specflow
{
    public static class HelperMethods
    {

        public static string GetAppSettingsValue(string key, bool emptyAllowed = false)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (!emptyAllowed && string.IsNullOrEmpty(value))
                throw new ArgumentException(string.Format("AppSetting {0} is required", key));

            return value;
        }
        public static bool IsLabel(this Label label, int lcid, string name)
        {
            return name.Equals(label.GetLabelInLanguage(lcid), StringComparison.CurrentCultureIgnoreCase);
        }

        public static T ExecuteWithRetry<T>(int retryCount, int sleepTime, Func<T> action)
        {
            try
            {
                return action();
            }
            catch
            {
                if (retryCount > 0)
                {
                    Thread.Sleep(sleepTime);
                    return ExecuteWithRetry(retryCount - 1, sleepTime, action);
                }
                else
                    throw;
            }
        }

        public static string GetLabelInLanguage(this Label label, int lcid)
        {
            string result = label.LocalizedLabels.Where(l => l.LanguageCode == lcid).FirstOrDefault()?.Label;

            if (label.UserLocalizedLabel != null && !string.IsNullOrEmpty(label.UserLocalizedLabel.Label) && string.IsNullOrEmpty(result))
                throw new ArgumentException(string.Format("Label {0} doesn't have a translation for language {1}", label.UserLocalizedLabel.Label, lcid));

            return result;
        }

        public static object CrmObjectToPrimitive(object value)
        {
            if (value == null)
                return null;

            var type = value.GetType();
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

    }
}
