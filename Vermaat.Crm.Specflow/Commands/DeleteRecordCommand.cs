using Microsoft.Xrm.Sdk;
using System;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    public class DeleteRecordCommand : BrowserCommand
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
            GlobalTestingContext.ConnectionManager.CurrentConnection.Delete(_toDelete);
            _crmContext.RecordCache.Remove(_alias);
        }

        protected override void ExecuteBrowser()
        {
            var formData = _seleniumContext.GetBrowser().OpenRecord(new OpenFormOptions(_toDelete));
            formData.CommandBar.Delete();
            _crmContext.RecordCache.Remove(_alias);
        }
    }
}
