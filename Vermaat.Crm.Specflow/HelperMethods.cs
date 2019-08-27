using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow
{
    public static class HelperMethods
    {

        public static string GetAppSettingsValue(string key, bool emptyAllowed = false)
        {
            string value = ConfigurationManager.AppSettings[key];

            if (!emptyAllowed && string.IsNullOrEmpty(value))
                throw new TestExecutionException(Constants.ErrorCodes.APP_SETTINGS_REQUIRED, key);

            return value;
        }
        public static bool IsLabel(this Label label, int lcid, string name)
        {
            return name.Equals(label.GetLabelInLanguage(lcid), StringComparison.CurrentCultureIgnoreCase);
        }

        public static void ExecuteWithRetry(int retryCount, int sleepTime, Action action)
        {
            ExecuteWithRetry(retryCount, sleepTime, () =>
            {
                action();
                return true;
            });
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
                throw new TestExecutionException(Constants.ErrorCodes.LABEL_NOT_TRANSLATED, label.UserLocalizedLabel.Label, lcid);

            return result;
        }

        public static object CrmObjectToPrimitive(object value)
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

        public static void WaitForFormLoad(IWebDriver driver, FormLoadConditions additionalConditionsToWaitFor = null)
        {
            DateTime timeout = DateTime.Now.AddSeconds(30);

            bool loadComplete = false;
            while(!loadComplete)
            {
                loadComplete = true;

                TimeSpan timeLeft = timeout.Subtract(DateTime.Now);
                if (timeLeft.TotalMilliseconds > 0)
                {
                    driver.WaitForPageToLoad();
                    driver.WaitUntilClickable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormLoad),
                        timeLeft,
                        null,
                        d => { throw new TestExecutionException(Constants.ErrorCodes.FORM_LOAD_TIMEOUT); }
                    );

                    if (additionalConditionsToWaitFor != null)
                    {

                        if (!string.IsNullOrEmpty(additionalConditionsToWaitFor.EntityName))
                        {
                            var entityName = driver.ExecuteScript("return Xrm.Page.data.entity.getEntityName();") as string;
                            if (string.IsNullOrEmpty(entityName) ||
                                !entityName.Equals(additionalConditionsToWaitFor.EntityName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                loadComplete = false;
                            }
                        }
                        if (additionalConditionsToWaitFor.RecordId.HasValue)
                        {
                            var entityId = driver.ExecuteScript("return Xrm.Page.data.entity.getId();") as string;
                            if(!Guid.TryParse(entityId, out Guid parsedId) || parsedId != additionalConditionsToWaitFor.RecordId.Value)
                            {
                                loadComplete = false;
                            }
                        }
                    }

                    if (!loadComplete)
                        Thread.Sleep(100);
                }
                else
                {
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_LOAD_TIMEOUT);
                }

            }           
        }

    }
}
