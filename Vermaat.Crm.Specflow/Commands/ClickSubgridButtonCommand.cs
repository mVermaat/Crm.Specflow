using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Commands
{
    public class ClickSubgridButtonCommand : BrowserOnlyCommand
    {
        private readonly string _parentAlias;

        private readonly string _subgridName;
        private readonly string _gridButtonId;

        public ClickSubgridButtonCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext,
            string parentAlias, string subgridName, string gridButtonId) : base(crmContext, seleniumContext)
        {
            _parentAlias = parentAlias;
            _subgridName = subgridName;
            _gridButtonId = gridButtonId;
        }

        public override void Execute()
        {
            var parentRecord = _crmContext.RecordCache.Get(_parentAlias, true);
            var parentMd = GlobalTestingContext.Metadata.GetEntityMetadata(parentRecord.LogicalName);

            var browser = _seleniumContext.GetBrowser();
            var record = browser.OpenRecord(parentMd, parentRecord);

            record.ClickSubgridButton(_subgridName, _gridButtonId);
        }
    }
}
