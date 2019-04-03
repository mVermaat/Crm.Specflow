using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Commands
{
    class AssertFormVisiblityCommand : BrowserOnlyCommand
    {
        private readonly EntityReference _crmRecord;
        private readonly Table _visibilityCriteria;

        public AssertFormVisiblityCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, EntityReference crmRecord, Table visibilityCriteria) :
            base(crmContext, seleniumContext)
        {
            _crmRecord = crmRecord;
            _visibilityCriteria = visibilityCriteria;
        }

        public override void Execute()
        {
            var formData = _seleniumContext.Browser.OpenRecord(_crmContext.Metadata.GetEntityMetadata(_crmRecord.LogicalName), _crmRecord);
            List<string> errors = new List<string>();

            foreach (TableRow row in _visibilityCriteria.Rows)
            {
                Assert.IsTrue(formData.ContainsField(row[Constants.SpecFlow.TABLE_KEY]), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                    

                bool expectedVisible = bool.Parse(row["Visible"]);
                if (formData[row[Constants.SpecFlow.TABLE_KEY]].IsVisible() != expectedVisible)
                {
                    errors.Add(string.Format("{0} was expected to be {1}visible but it is {2}visible",
                        row[Constants.SpecFlow.TABLE_KEY], expectedVisible ? "" : "in", expectedVisible ? "in" : ""));
                }
            }
            Assert.AreEqual(0, errors.Count, string.Join(", ", errors));
        }
    }
}
