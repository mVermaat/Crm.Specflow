using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    class AssertFormVisiblityCommand : BrowserOnlyCommand
    {
        private readonly EntityReference _crmRecord;
        private readonly Table _visibilityCriteria;

        public AssertFormVisiblityCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, EntityReference crmRecord, Table visibilityCriteria) : 
            base(crmContext, seleniumContext)
        {
            this._crmRecord = crmRecord;
            this._visibilityCriteria = visibilityCriteria;
        }

        public override void Execute()
        {
            NavigationHelper.OpenRecord(_seleniumContext.Browser, _crmRecord);
            var errors = new List<string>();
            foreach (var row in _visibilityCriteria.Rows)
            {
                var expectedVisible = bool.Parse(row["Visible"]);
                if (_seleniumContext.Browser.Entity.IsElementVisible(row[Constants.SpecFlow.TABLE_KEY]) != expectedVisible)
                {
                    errors.Add(string.Format("{0} was expected to be {1}visible but it is {2}visible",
                        row[Constants.SpecFlow.TABLE_KEY], expectedVisible ? "" : "in", expectedVisible ? "in" : ""));
                }
            }
            Assert.AreEqual(0, errors.Count, string.Join(", ", errors));
        }
    }
}
