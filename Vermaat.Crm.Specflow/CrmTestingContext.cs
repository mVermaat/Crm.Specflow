using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow
{
    [Binding]
    public class CrmTestingContext
    {

        public RecordBuilder RecordBuilder { get; }
        public TableConverter TableConverter { get; }

        public CommandProcessor CommandProcessor { get; set; }

        public AliasedRecordCache RecordCache { get; }

        private readonly string[] _targets;

        public int LanguageCode { get; set; }

        public CrmTestingContext(ScenarioContext scenarioContext)
        {
            RecordBuilder = new RecordBuilder(this);
            TableConverter = new TableConverter(this);
            LanguageCode = GetLanguageCode();
            CommandProcessor = new CommandProcessor(scenarioContext);
            RecordCache = new AliasedRecordCache(GlobalTestingContext.ConnectionManager, GlobalTestingContext.Metadata);

            _targets = ConfigurationManager.AppSettings["Target"]
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
            if (!int.TryParse(HelperMethods.GetAppSettingsValue("LanguageCode"), out int lcid))
                throw new TestExecutionException(Constants.ErrorCodes.LANGUAGECODE_MUST_BE_INTEGER);

            return lcid;
        }

        
    }
}
