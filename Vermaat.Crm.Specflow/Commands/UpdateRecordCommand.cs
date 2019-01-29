using Microsoft.Xrm.Sdk;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    class UpdateRecordCommand : BrowserCommand
    {
        private readonly EntityReference _toUpdate;
        private readonly Table _criteria;

        public UpdateRecordCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, EntityReference toUpdate,
            Table criteria)
            : base(crmContext, seleniumContext)
        {
            _toUpdate = toUpdate;
            _criteria = criteria;
        }

        protected override void ExecuteApi()
        {
            Entity toUpdate = new Entity(_toUpdate.LogicalName)
            {
                Id = _toUpdate.Id
            };

            foreach (TableRow row in _criteria.Rows)
            {
                toUpdate[row[Constants.SpecFlow.TABLE_KEY]] = ObjectConverter.ToCrmObject(_toUpdate.LogicalName,
                    row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE], _crmContext);
            }

            _crmContext.Service.Update(toUpdate);
        }

        protected override void ExecuteBrowser()
        {
            _seleniumContext.Browser.Entity.OpenEntity(_toUpdate.LogicalName, _toUpdate.Id);
            FormHelper.FillForm(_crmContext, _seleniumContext.Browser, _toUpdate.LogicalName, _criteria);
            FormHelper.SaveRecord(_seleniumContext, true);
        }
    }
}
