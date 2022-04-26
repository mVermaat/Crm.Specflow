using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public partial class LocalizedTexts
    {
        public LocalizedTexts()
        {
            SaveAndCloseButton = "Save & Close";
            NewButton = "New";
            DeleteButton = "Delete";
            SaveButton = "Save (CTRL+S)";
            ActivateQuoteButton = "Activate Quote";
            CreateOrderButton = "Create Order";
            ReviseQuoteButton = "Revise";
            CloseQuoteButton = "Close Quote";
            SaveStatusSaving = "saving";
            SaveStatusUnsaved = "unsaved;";
        }

        public string SaveAndCloseButton { get; set; }
        public string NewButton { get; set; }
        public string DeleteButton { get; set; }
        public string SaveButton { get; set; }
        public string ActivateQuoteButton { get; set; }
        public string CreateOrderButton { get; set; }
        public string ReviseQuoteButton { get; set; }
        public string CloseQuoteButton { get; set; }

        public string SaveStatusSaving { get; set; }
        public string SaveStatusUnsaved { get; set; }

        public static LocalizedTexts FromOverrideFile()
        {
            var overridesJsonPath = HelperMethods.GetAppSettingsValue("LocalizationOverrides", true);
            if (string.IsNullOrWhiteSpace(overridesJsonPath))
                return new LocalizedTexts();

            FileInfo dllPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
            FileInfo fileInfo = new FileInfo(Path.Combine(dllPath.DirectoryName, overridesJsonPath));

            if (!fileInfo.Exists)
                throw new TestExecutionException(Constants.ErrorCodes.LOCALIZATION_OVERRIDES_NOT_FOUND, fileInfo.FullName);

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.DeserializeObject<LocalizedTexts>(File.ReadAllText(fileInfo.FullName), settings);
        }
    }
}
