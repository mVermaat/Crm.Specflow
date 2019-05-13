using Microsoft.Xrm.Sdk;
using System;

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
            _crmContext.Service.Delete(_toDelete);
            _crmContext.RecordCache.Remove(_alias);
        }

        protected override void ExecuteBrowser()
        {
            var formData = _seleniumContext.Browser.OpenRecord(_crmContext.Metadata.GetEntityMetadata(_toDelete.LogicalName), _toDelete.LogicalName, _toDelete.Id);
            formData.CommandBar.Delete();
            _crmContext.RecordCache.Remove(_alias);
        }
    }
}
