using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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
    public class CreateRelatedRecordCommand : BrowserCommandFunc<EntityReference>
    {
        private readonly string _entityLogicalName;
        private readonly Table _criteria;
        private readonly string _alias;
        private readonly string _parentAlias;

        public CreateRelatedRecordCommand(ICrmContext crmContext, ISeleniumContext seleniumContext,
       string entityLogicalName, Table criteria, string alias, string parentAlias)
       : base(crmContext, seleniumContext)
        {
            _entityLogicalName = entityLogicalName;
            _criteria = criteria;
            _alias = alias;
            _parentAlias = parentAlias;
        }

        protected override EntityReference ExecuteApi()
        {
            var parentRecord = _crmContext.RecordCache.Get(_parentAlias, true);
            var mappings = GlobalContext.Metadata.GetAttributeMaps(parentRecord.LogicalName, _entityLogicalName);

            Entity toCreate = _crmContext.RecordBuilder.SetupEntityFromParent(parentRecord, _entityLogicalName, _criteria, mappings);
            GlobalContext.ConnectionManager.CurrentConnection.Create(toCreate, _alias, _crmContext.RecordCache);
            return toCreate.ToEntityReference();
        }

        protected override EntityReference ExecuteBrowser()
        {
            var formData = GlobalContext.ConnectionManager
                .GetCurrentBrowserSession(_seleniumContext)
                .OpenRecord(new OpenFormOptions(_entityLogicalName)
            {
                Parent = _crmContext.RecordCache.Get(_parentAlias, true)
            });

            formData.FillForm(_crmContext, _criteria);
            formData.Save(true);

            var record = new EntityReference(_entityLogicalName, formData.GetRecordId());
            _crmContext.RecordCache.Add(_alias, record);
            return record;
        }

    }
}
