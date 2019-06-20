using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Commands
{
    public class AssertCrmRecordCommand : ApiOnlyCommand
    {
        private readonly EntityReference _crmRecord;
        private readonly Table _criteria;

        public AssertCrmRecordCommand(CrmTestingContext crmContext, EntityReference crmRecord, Table criteria) : base(crmContext)
        {
            _crmRecord = crmRecord;
            _criteria = criteria;
        }

        public override void Execute()
        {
            ColumnSet columns = new ColumnSet(_criteria.Rows.Select(r => r[Constants.SpecFlow.TABLE_KEY]).ToArray());
            Entity record = GlobalTestingContext.ConnectionManager.CurrentConnection.Retrieve(_crmRecord, columns);
            AssertHelper.HasProperties(record, _criteria, _crmContext);
        }
    }
}
