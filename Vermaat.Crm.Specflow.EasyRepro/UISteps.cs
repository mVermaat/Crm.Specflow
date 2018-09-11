using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    [Binding]
    public class UISteps
    {

        private CrmTestingContext _crmContext;
        private SeleniumTestingContext _seleniumContext;


        public UISteps(SeleniumTestingContext seleniumContext, CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        [When("I login to CRM")]
        public void Login()
        {
            _seleniumContext.Browser.LoginPage.Login(new Uri(
                _crmContext.ConnectionInfo.Url),
                _crmContext.ConnectionInfo.Username.ToSecureString(),
                _crmContext.ConnectionInfo.Password.ToSecureString());
        }

        [When(@"I open ([^\s]+)")]
        public void OpenRecordViaUrl(string alias)
        {
            var record = _crmContext.RecordCache[alias];
            _seleniumContext.Browser.Entity.OpenEntity(record.LogicalName, record.Id);
        }

        [When(@"I enter the following data in the form")]
        public void FillForm(Table dataTable)
        {
            // Getting this field switches the context to the form frame and allows Xrm.Page functions
            var entityFunctions = _seleniumContext.Browser.Entity;
            FormHelper.FillForm(_crmContext, entityFunctions,
                FormHelper.GetEntityName(_seleniumContext.Browser.Driver), dataTable);
        }

        [When(@"I save the record")]
        public void SaveForm()
        {
            var entity = _seleniumContext.Browser.Entity;
            entity.Save();
            _seleniumContext.Browser.Dialogs.DuplicateDetection(true);
            entity.SwitchToContentFrame();
        }

        [When(@"I save the record and name it (.*)")]
        public void SaveFormWithAlias(string alias)
        {
            SaveForm();
            FormHelper.AddAlias(_crmContext, _seleniumContext.Browser.Driver, alias);
        }

        [When(@"I save and close the record")]
        public void SaveAndCloseForm()
        {
            _seleniumContext.Browser.CommandBar.ClickCommand(_seleniumContext.ButtonTexts.SaveAndClose);
            _seleniumContext.Browser.Dialogs.DuplicateDetection(true);
        }

        [When("I navigate to '(.*)' and '(.*)'")]
        public void NavigateToArea(string area, string subArea)
        {
            _seleniumContext.Browser.Navigation.OpenSubArea(area, subArea);
        }

        [When("I open the new record form")]
        public void CreateNewRecord()
        {
            _seleniumContext.Browser.CommandBar.ClickCommand(_seleniumContext.ButtonTexts.New);
        }
    }
}
