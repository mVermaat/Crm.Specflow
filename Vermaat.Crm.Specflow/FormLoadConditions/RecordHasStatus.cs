using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using OpenQA.Selenium;

namespace Vermaat.Crm.Specflow.FormLoadConditions
{
    public class RecordHasStatus : IFormLoadCondition
    {
        private readonly EntityReference _record;
        private readonly int _statecode;

        public RecordHasStatus(EntityReference record, int statecode)
        {
            _record = record;
            _statecode = statecode;
        }

        public bool Evaluate(IWebDriver driver)
        {
            Logger.WriteLine($"Evaluating if current record's statecode is {_statecode}");

            var record = GlobalTestingContext.ConnectionManager.AdminConnection.Retrieve(_record, new ColumnSet("statecode"));
            return _statecode.Equals(record.GetAttributeValue<OptionSetValue>("statecode")?.Value);

        }
    }
}
