using Microsoft.Xrm.Sdk;
using System;

namespace Vermaat.Crm.Specflow.Commands
{
    class DeleteRecordCommand : BrowserCommand
    {
        private readonly EntityReference _toDelete;
        private readonly string _alias;

        public DeleteRecordCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, string alias)
            :base(crmContext, seleniumContext)
        {
            _toDelete = crmContext.RecordCache.Get(alias, true);
            _alias = alias;
        }

        protected override void ExecuteApi()
        {
            _crmContext.Service.Delete(_toDelete);
            _crmContext.RecordCache.Remove(_alias);
        }

        protected override void ExecuteBrowser()
        {
            _seleniumContext.Browser.OpenRecord(_toDelete.LogicalName, _toDelete.Id);
            _seleniumContext.Browser.Entity.DeleteRecord();
            _crmContext.RecordCache.Remove(_alias);
        }
    }
}
