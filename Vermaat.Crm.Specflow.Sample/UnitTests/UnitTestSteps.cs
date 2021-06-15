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
        private readonly SeleniumTestingContext _seleniumContext;

        public UnitTestSteps(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        [Then("The command action is set to (.*)")]
        public void ThenCommandActionIsSetTo(string commandAction)
        {
            var action = (CommandAction)Enum.Parse(typeof(CommandAction), commandAction, true);

            Assert.AreEqual(action, _crmContext.CommandProcessor.DefaultCommandAction);
        }

        [When(@"([^\s]+) has a contact named ([^\s]+) created via quick create with the following values")]
        public void QuickCreateContact(string accountAlias, string contactAlias, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable("contact", criteria);
            _crmContext.CommandProcessor.Execute(new QuickCreateTestCommand(_crmContext, _seleniumContext, contactAlias, accountAlias, criteria));
        }
    }
}
