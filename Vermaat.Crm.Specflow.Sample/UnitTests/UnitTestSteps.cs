using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Sample.UnitTests
{
    [Binding]
    public class UnitTestSteps 
    {
        private readonly CrmTestingContext _crmContext;

        public UnitTestSteps(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
        }

        [Then("The command action is set to (.*)")]
        public void ThenCommandActionIsSetTo(string commandAction)
        {
            var action = (CommandAction)Enum.Parse(typeof(CommandAction), commandAction, true);

            Assert.AreEqual(action, _crmContext.CommandProcessor.DefaultCommandAction);
        }
    }
}
