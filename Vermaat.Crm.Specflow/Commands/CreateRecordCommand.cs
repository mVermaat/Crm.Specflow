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
            NavigationHelper.OpenNewForm(_seleniumContext.Browser, _entityLogicalName);
            FormHelper.FillForm(_crmContext, _seleniumContext.Browser, _entityLogicalName, _criteria);
            FormHelper.SaveRecord(_seleniumContext, true);
            return FormHelper.AddAlias(_crmContext, _seleniumContext.Browser.Driver, _alias);
        }
    }
}
