using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public partial class LocalizedTexts
    {
        private Dictionary<string, Dictionary<string, string>> _localizedTexts;

        public LocalizedTexts()
        {
            _localizedTexts = LoadOverrideFile();
        }

        /// <summary>
        /// Gets a localized text
        /// </summary>
        /// <param name="key">Key of the text (Constants.LocalizedTexts or a custom class)</param>
        /// <returns></returns>
        public string this[string key, int lcid]
        {
            get
            {
                if (!_localizedTexts.TryGetValue(lcid.ToString(), out var textsForLcid))
                    throw new TestExecutionException();
                if (textsForLcid.TryGetValue(key, out var localizedText))
                    return localizedText;
                else
                    return null;
            }
        }


        private Dictionary<string, Dictionary<string, string>> LoadOverrideFile()
        {
            var overridesJsonPath = HelperMethods.GetAppSettingsValue("LocalizationOverrides", true);
            if (string.IsNullOrWhiteSpace(overridesJsonPath))
            {
                Logger.WriteLine("No localization found. Using Defaults");
                return new Dictionary<string, Dictionary<string, string>>()
                {
                    { HelperMethods.GetAppSettingsValue("LanguageCode"), GetDefaults() }
                };
            }

            Logger.WriteLine($"Localization found: {overridesJsonPath}");
            FileInfo dllPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
            FileInfo fileInfo = new FileInfo(Path.Combine(dllPath.DirectoryName, overridesJsonPath));

            if (!fileInfo.Exists)
                throw new TestExecutionException(Constants.ErrorCodes.LOCALIZATION_OVERRIDES_NOT_FOUND, fileInfo.FullName);

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var localizedTextSet = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(fileInfo.FullName), settings);

            foreach (var key in localizedTextSet.Keys.ToArray())
            {
                localizedTextSet[key] = MergeObjects(GetDefaults(), localizedTextSet[key]);
            }

            return localizedTextSet;
        }

        private Dictionary<string, string> MergeObjects(Dictionary<string, string> localizedTexts, Dictionary<string, string> overrides)
        {
            foreach (var item in overrides)
            {
                if (localizedTexts.ContainsKey(item.Key))
                    localizedTexts[item.Key] = item.Value;
                else
                    localizedTexts.Add(item.Key, item.Value);
            }
            return localizedTexts;
        }

        private Dictionary<string, string> GetDefaults()
            => new Dictionary<string, string>()
            {
                { Constants.LocalizedTexts.ActivateQuoteButton, "Activate Quote" },
                { Constants.LocalizedTexts.CloseAsLost, "Close as Lost" },
                { Constants.LocalizedTexts.CloseAsWon, "Close as Won" },
                { Constants.LocalizedTexts.CreateOrderButton, "Create Order" },
                { Constants.LocalizedTexts.DeleteButton, "Delete" },
                { Constants.LocalizedTexts.ReviseQuoteButton, "Revise" },
                { Constants.LocalizedTexts.SaveButton, "Save" },
                { Constants.LocalizedTexts.SaveStatusSaved, "saved" },
                { Constants.LocalizedTexts.SaveStatusSaving, "saving" },
                { Constants.LocalizedTexts.SaveStatusUnsaved, "unsaved" },
                { Constants.LocalizedTexts.QuickCreateViewRecord, "View Record" },
            };
    }
}
