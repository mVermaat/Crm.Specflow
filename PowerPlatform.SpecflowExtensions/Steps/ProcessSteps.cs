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
    public class ProcessSteps
    {
        private readonly ICrmContext _crmContext;
        private readonly ISeleniumContext _seleniumContext;

        public ProcessSteps(ICrmContext crmContext, ISeleniumContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        [When(@"the workflow '(.*)' is executed on (.*)")]
        public void ExecuteWorkflow(string workflowName, string alias)
        {
            _crmContext.CommandProcessor.Execute(new RunOnDemandWorkflowCommand(_crmContext, workflowName, alias));
        }
    }
}
