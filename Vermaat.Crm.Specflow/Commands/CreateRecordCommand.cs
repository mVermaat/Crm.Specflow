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
            _crmContext.Service.Create(toCreate, _alias);
            return toCreate.ToEntityReference();
        }

        protected override EntityReference ExecuteBrowser()
        {
            var formData = _seleniumContext.Browser.OpenRecord(_crmContext.Metadata.GetEntityMetadata(_entityLogicalName), _entityLogicalName);
            formData.FillForm(_crmContext, _criteria);
            formData.Save(true);

            var record = new EntityReference(_entityLogicalName, formData.GetRecordId());
            _crmContext.RecordCache.Add(_alias, record);
            return record;
        }
    }
}
