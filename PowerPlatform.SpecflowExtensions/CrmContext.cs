using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions
{
    public class CrmContext : ICrmContext
    {
        private readonly string[] _targets;

        public CommandProcessor CommandProcessor { get; private set; }
        public AliasedRecordCache RecordCache { get; private set; }
        public int LanguageCode { get; private set; }
        public TableConverter TableConverter { get; private set; }

        public CrmContext(ScenarioContext scenarioContext)
        {
            LanguageCode = GetLanguageCode();
            RecordCache = new AliasedRecordCache(GlobalContext.Metadata);
            CommandProcessor = new CommandProcessor(scenarioContext);

            _targets = ConfigurationManager.AppSettings[Constants.AppSettings.TARGET]
                .ToLower()
                .Split(';')
                .Select(splitted => splitted.Trim())
                .ToArray();
        }

        public bool IsTarget(string target)
        {
            if (string.IsNullOrWhiteSpace(target))
                return false;

            return _targets.Contains(target.ToLower());
        }

        private int GetLanguageCode()
        {
            if (!int.TryParse(HelperMethods.GetAppSettingsValue(Constants.AppSettings.LANGUAGE_CODE), out int lcid))
                throw new TestExecutionException(Constants.ErrorCodes.LANGUAGECODE_MUST_BE_INTEGER);

            return lcid;
        }
    }
}
