using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions
{
    public class CrmContext : ICrmContext
    {
        private readonly ScenarioContext _scenarioContext;

        public AliasedRecordCache RecordCache { get; private set; }
        public int LanguageCode { get; private set; }
        public TableConverter TableConverter { get; private set; }

        public CrmContext(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            LanguageCode = GetLanguageCode();
            RecordCache = new AliasedRecordCache(GlobalContext.Metadata);
        }

        private int GetLanguageCode()
        {
            if (!int.TryParse(HelperMethods.GetAppSettingsValue(Constants.AppSettings.LANGUAGE_CODE), out int lcid))
                throw new TestExecutionException(Constants.ErrorCodes.LANGUAGECODE_MUST_BE_INTEGER);

            return lcid;
        }
    }
}
