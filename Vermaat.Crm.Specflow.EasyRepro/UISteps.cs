using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro.Extensions;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    [Binding]
    public class UISteps
    {

        private CrmTestingContext _crmContext;
        private SeleniumTestingContext _seleniumContext;


        public UISteps(SeleniumTestingContext seleniumContext, CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        [Then("(.*)'s form has the following fields (in)visible on the form")]
        public void ThenFieldsAreVisibleOnForm(string alias, Table table)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            NavigationHelper.OpenRecord(_seleniumContext.Browser, aliasRef);

            var errors = new List<string>();
            foreach(var row in table.Rows)
            {
                var expectedVisible = Boolean.Parse(row["Visible"];
                if (_seleniumContext.Browser.Entity.IsElementVisible(row["Field"]) != expectedVisible)
                {
                    errors.Add(string.Format("{0} was expected to be {1}visible but it is {2}visible",
                        row["Field"], expectedVisible ? "" : "in", expectedVisible ? "in" : ""));
                }

                throw new NotImplementedException(); // nuget not working without internet
                //Assert.AreEqual(0, errors.Count, string.Join(", ", errors));
            }

        }
    }
}
