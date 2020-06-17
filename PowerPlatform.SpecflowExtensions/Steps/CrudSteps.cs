using FluentAssertions;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Steps
{
    [Binding]
    public class CrudSteps
    {
        private readonly ICrmContext _crmContext;

        public CrudSteps(ICrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        [Given(@"a ([^\s]+) named (.*) with the following values")]
        [Given(@"an ([^\s]+) named (.*) with the following values")]
        [When(@"a ([^\s]+) named (.*) is created with the following values")]
        [When(@"an ([^\s]+) named (.*) is created with the following values")]
        public void GivenEntityWithValues(string entityName, string alias, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);
            //_crmContext.CommandProcessor.Execute(new CreateRecordCommand(_crmContext, _seleniumContext, entityName, criteria, alias));
        }

    }
}
