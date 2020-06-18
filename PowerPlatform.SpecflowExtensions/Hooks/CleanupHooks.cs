using OpenQA.Selenium;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Hooks
{
    [Binding]
    public class CleanupHooks
    {
        private readonly ICrmContext _crmContext;

        public CleanupHooks(ICrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        [AfterScenario("Cleanup")]
        public void Cleanup()
        {
            _crmContext.RecordCache.DeleteAllCachedRecords();
        }




    }
}
