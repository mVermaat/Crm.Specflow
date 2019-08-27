using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    public class ClickSubgridButtonCommand : BrowserOnlyCommand
    {
        private readonly string _parentAlias;
        private readonly string _tabName;
        private readonly string _subgridName;
        private readonly string _gridButtonId;

        public ClickSubgridButtonCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext,
            string parentAlias, string tabName, string subgridName, string gridButtonId) : base(crmContext, seleniumContext)
        {
            _parentAlias = parentAlias;
            _tabName = tabName;
            _subgridName = subgridName;
            _gridButtonId = gridButtonId;
        }

        public override void Execute()
        {
            var parentRecord = _crmContext.RecordCache.Get(_parentAlias, true);

            var browser = _seleniumContext.GetBrowser();
            var record = browser.OpenRecord(new OpenFormOptions(parentRecord));
            record.ExpandTab(_tabName);

            Logger.WriteLine($"Clicking button '{_gridButtonId} in grid {_subgridName}");
            record.ClickSubgridButton(_subgridName, _gridButtonId);
        }
    }
}
