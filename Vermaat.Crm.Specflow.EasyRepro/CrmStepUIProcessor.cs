using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro.Extensions;
using Vermaat.Crm.Specflow.Processors;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    class CrmStepUIProcessor : CrmStepProcessor
    {
        private readonly SeleniumTestingContext _seleniumContext;

        public CrmStepUIProcessor(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext)
            :base(crmContext)
        {
            _seleniumContext = seleniumContext;
        }

        public override void CreateAliasedRecord(string entityLogicalName, Table table, string alias)
        {
            NavigationHelper.OpenNewForm(_seleniumContext.Browser, entityLogicalName);
            FormHelper.FillForm(CrmContext, _seleniumContext.Browser, entityLogicalName, table);
            FormHelper.SaveRecord(_seleniumContext, true);
            FormHelper.AddAlias(CrmContext, _seleniumContext.Browser.Driver, alias);
        }

        public override void DeleteRecord(EntityReference crmRecord)
        {
            _seleniumContext.Browser.Entity.OpenEntity(crmRecord.LogicalName, crmRecord.Id);
            _seleniumContext.Browser.Entity.ClickCommand(_seleniumContext.ButtonTexts.Delete);
            _seleniumContext.Browser.Dialogs.Delete();
        }

        public override void UpdateRecord(EntityReference crmRecord, Table table)
        {
            _seleniumContext.Browser.Entity.OpenEntity(crmRecord.LogicalName, crmRecord.Id);
            FormHelper.FillForm(CrmContext, _seleniumContext.Browser, crmRecord.LogicalName, table);
            FormHelper.SaveRecord(_seleniumContext, true);
        }
    }
}
