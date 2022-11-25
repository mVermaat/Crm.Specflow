using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow.Steps
{
    [Binding]
    public class EnvironmentVariableSteps
    {
        private readonly CrmTestingContext _crmContext;

        public EnvironmentVariableSteps(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
        }

        [Given(@"the environment variable ([^\s]+) has the value ([^\s]+)")]
        public void GivenEnvironmentVariableWithValue(string variableName, string value)
        {
            _crmContext.CommandProcessor.Execute(new SetEnvironmentVariableCommand(_crmContext, variableName, value));
        }

    }
}
