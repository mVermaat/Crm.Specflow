using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    public class CreateRecordCommand : BrowserCommandFunc<EntityReference>
    {
        private readonly string _entityLogicalName;
        private readonly Table _criteria;
        private readonly string _alias;

        public CreateRecordCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext,
            string entityLogicalName, Table criteria, string alias)
            : base(crmContext, seleniumContext)
        {
            _entityLogicalName = entityLogicalName;
            _criteria = criteria;
            _alias = alias;
        }

        protected override EntityReference ExecuteApi()
        {
            Entity toCreate = _crmContext.RecordBuilder.SetupEntityWithDefaults(_entityLogicalName, _criteria);
            GlobalTestingContext.ConnectionManager.CurrentConnection.Create(toCreate, _alias, _crmContext.RecordCache);
            return toCreate.ToEntityReference();
        }

        protected override EntityReference ExecuteBrowser()
        {
            var formData = _seleniumContext.GetBrowser().OpenRecord(new OpenFormOptions(_entityLogicalName));

            var tableWithDefaults = _crmContext.RecordBuilder.AddDefaultsToTable(_entityLogicalName, _criteria);

            formData.FillForm(_crmContext, tableWithDefaults);
            formData.Save(true);

            var record = new EntityReference(_entityLogicalName, formData.GetRecordId());
            _crmContext.RecordCache.Add(_alias, record);
            return record;
        }
    }
}
