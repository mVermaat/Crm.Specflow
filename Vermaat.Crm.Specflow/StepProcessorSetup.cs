using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Processors;

namespace Vermaat.Crm.Specflow
{
    [Binding]
    public class StepProcessorSetup
    {
        private readonly CrmTestingContext _crmContext;
        private readonly StepProcessor _processor;

        public StepProcessorSetup(CrmTestingContext crmContext, StepProcessor processor)
        { 
            _crmContext = crmContext;
            _processor = processor;
        }

        [BeforeScenario("API")]
        public void SetupAPISteps()
        {
            if(ScenarioContext.Current.IsTagTargetted("API"))
            {
                _processor.SetDefaultProcessors(_crmContext);
            }
        }
    }
}
