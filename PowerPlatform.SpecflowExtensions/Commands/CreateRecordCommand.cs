using BoDi;
using Microsoft.Xrm.Sdk;
using PowerPlatform.SpecflowExtensions.EasyRepro;
using PowerPlatform.SpecflowExtensions.EasyRepro.Apps;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class CreateRecordCommand : BrowserCommandFunc<EntityReference>
    {
        private readonly string _entityLogicalName;
        private readonly Table _criteria;
        private readonly string _alias;

        public CreateRecordCommand(IObjectContainer container,
            string entityLogicalName, Table criteria, string alias)
            : base(container)
        {
            _entityLogicalName = entityLogicalName;
            _criteria = criteria;
            _alias = alias;
        }

        protected override EntityReference ExecuteApi()
        {
            Entity toCreate = _crmContext.RecordBuilder.SetupEntityWithDefaults(_entityLogicalName, _criteria);
            GlobalContext.ConnectionManager.CurrentCrmService.Create(toCreate, _alias, _crmContext.RecordCache);
            return toCreate.ToEntityReference();
        }

        protected override EntityReference ExecuteBrowser()
        {
            var form = GlobalContext.ConnectionManager
                .GetCurrentBrowserSession(_seleniumContext)
                .GetApp<CustomerEngagementApp>(_container)
                .OpenRecord(new OpenFormOptions(_entityLogicalName));

            var tableWithDefaults = _crmContext.RecordBuilder.AddDefaultsToTable(_entityLogicalName, _criteria);
            form.FillForm(_crmContext, tableWithDefaults);
            form.Save(true);

            var record = new EntityReference(_entityLogicalName, form.GetRecordId());
            _crmContext.RecordCache.Add(_alias, record);
            return record;
        }
    }
}
