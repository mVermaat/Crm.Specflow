using Microsoft.Xrm.Sdk;
using PowerPlatform.SpecflowExtensions.EasyRepro;
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

        public CreateRecordCommand(ICrmContext crmContext, ISeleniumContext seleniumContext,
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
            GlobalContext.ConnectionManager.CurrentConnection.Create(toCreate, _alias, _crmContext.RecordCache);
            return toCreate.ToEntityReference();
        }

        protected override EntityReference ExecuteBrowser()
        {
            var form = GlobalContext.ConnectionManager
                .GetCurrentBrowserSession(_seleniumContext)
                .OpenRecord(new OpenFormOptions(_entityLogicalName));

            var tableWithDefaults = _crmContext.RecordBuilder.AddDefaultsToTable(_entityLogicalName, _criteria);
            form.FillForm(_crmContext, tableWithDefaults);


            //var tableWithDefaults = _crmContext.RecordBuilder.AddDefaultsToTable(_entityLogicalName, _criteria);

            //formData.FillForm(_crmContext, tableWithDefaults);
            //formData.Save(true);

            //var record = new EntityReference(_entityLogicalName, formData.GetRecordId());
            //_crmContext.RecordCache.Add(_alias, record);
            //return record;
            throw new NotImplementedException();
        }
    }
}
