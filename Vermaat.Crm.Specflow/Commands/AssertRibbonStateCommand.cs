using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    public class AssertRibbonStateCommand : BrowserOnlyCommand
    {
        private readonly string _alias;
        private readonly Table _ribbonState;

        public AssertRibbonStateCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, string alias, Table ribbonState) 
            : base(crmContext, seleniumContext)
        {
            _alias = alias;
            _ribbonState = ribbonState;
        }

        public override void Execute()
        {
            var recordRef = _crmContext.RecordCache.Get(_alias, true);
            var form = _seleniumContext.GetBrowser().OpenRecord(new OpenFormOptions(recordRef));

            List<string> errors = new List<string>();
            foreach(var row in _ribbonState.Rows)
            {
                var actual = form.CommandBar.IsButtonAvailable(row[Constants.SpecFlow.TABLE_KEY]);
                var expected = GetExpected(row[Constants.SpecFlow.TABLE_FORMSTATE]);
                
                if(expected != actual)
                {
                    errors.Add(string.Format("Ribbon button {0} was expected to be {1}visible, but is {2}visible",
                        row[Constants.SpecFlow.TABLE_KEY],
                        expected ? "" : "in",
                        expected ? "in" : ""));
                }
            }
        }

        private bool GetExpected(string expectedText)
        {
            if (string.IsNullOrEmpty(expectedText))
                throw new TestExecutionException(19, expectedText);
            else if (expectedText.Equals("visible", StringComparison.CurrentCultureIgnoreCase))
                return true;
            else if (expectedText.Equals("invisible", StringComparison.CurrentCultureIgnoreCase))
                return false;
            else
                throw new TestExecutionException(19, expectedText);

        }
    }
}
