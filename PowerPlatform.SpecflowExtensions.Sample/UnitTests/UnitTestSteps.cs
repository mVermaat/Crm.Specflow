using FluentAssertions;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Sample.UnitTests
{
    [Binding]
    public class UnitTestSteps
    {
        private readonly ICrmContext _crmContext;

        public UnitTestSteps(ICrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        [Then("The alias cache has the following records")]
        public void AssertAliasCache(Table aliasTable)
        {
            foreach(var row in aliasTable.Rows)
            {
                _crmContext.RecordCache.Get(row[Constants.SpecFlow.TABLE_VALUE]).Should().NotBeNull($"Alias {row[Constants.SpecFlow.TABLE_VALUE]} should be in the cache");
            }
        }
    }
}
