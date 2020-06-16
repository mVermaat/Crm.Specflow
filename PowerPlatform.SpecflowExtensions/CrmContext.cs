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

        public CrmContext(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            RecordCache = new AliasedRecordCache(GlobalContext.Metadata);
        }

        public AliasedRecordCache RecordCache { get; private set; }
    }
}
