using Vermaat.Crm.Specflow.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;
using Microsoft.Xrm.Sdk;
using Vermaat.Crm.Specflow.FormLoadConditions;

namespace Vermaat.Crm.Specflow.Sample.UnitTests
{
    class QuickCreateTestCommand : BrowserOnlyCommand
    {
        private readonly string _contactAlias;
        private readonly string _accountAlias;
        private readonly Table _contactCriteria;

        public QuickCreateTestCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext,
            string contactAlias, string accountAlias, Table contactCriteria)
            : base(crmContext, seleniumContext)
        {
            _contactAlias = contactAlias;
            _accountAlias = accountAlias;
            _contactCriteria = contactCriteria;
        }

        public override void Execute()
        {
            var browser = _seleniumContext.GetBrowser();

            var summaryTabLabel = GlobalTestingContext.LocalizedTexts["AccountSummaryTab", GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.UILanguage] ?? "Summary";
            var text = browser.App.LocalizedTexts[Constants.LocalizedTexts.QuickCreateViewRecord, browser.App.UILanguageCode];
            _crmContext.CommandProcessor.Execute(new ClickSubgridButtonCommand(_crmContext, _seleniumContext, _accountAlias,
                summaryTabLabel, "Contacts", "Mscrm.SubGrid.contact.AddNewStandard"));
            HelperMethods.WaitForFormLoad(browser.App.WebDriver, new FormIsOfEntity("contact"));

            var formData = browser.GetQuickFormData(GlobalTestingContext.Metadata.GetEntityMetadata("contact"));

            formData.FillForm(_crmContext, _contactCriteria);
            formData.Save(true);

            var childFormData = formData.OpenCreatedRecord(browser, "contact");
            var record = new EntityReference("contact", childFormData.GetRecordId());
            _crmContext.RecordCache.Add(_contactAlias, record);
        }
    }
}
