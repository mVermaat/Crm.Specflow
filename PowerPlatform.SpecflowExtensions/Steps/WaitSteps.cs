using PowerPlatform.SpecflowExtensions.Commands;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Steps
{
    [Binding]
    public class WaitSteps
    {
        private readonly ICrmContext _crmContext;

        public WaitSteps(ICrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        [When(@"all asynchronous processes for (.*) are finished")]
        public void WhenAsyncJobsFinished(string alias)
        {
            _crmContext.CommandProcessor.Execute(new WaitForAsyncJobsCommand(_crmContext, alias));
        }
    }
}
