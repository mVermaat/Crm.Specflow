using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Commands;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Steps
{
    [Binding]
    public class UISteps
    {

        private readonly CrmTestingContext _crmContext;
        private readonly SeleniumTestingContext _seleniumContext;


        public UISteps(SeleniumTestingContext seleniumContext, CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        [Then("(.*)'s form has the following form state")]
        public void ThenFieldsAreVisibleOnForm(string alias, Table table)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _crmContext.TableConverter.ConvertTable(aliasRef.LogicalName, table);
           
            _crmContext.CommandProcessor.Execute(new AssertFormStateCommand(_crmContext, _seleniumContext, aliasRef, table));

        }

        [Then(@"(.*) has the following form notifications")]
        public void ThenFormNotificationExist(string alias, Table formNotifications)
        {
            _crmContext.CommandProcessor.Execute(new AssertFormNotificationsCommand(_crmContext, _seleniumContext, alias, formNotifications));
        }

        [Then(@"the following form notifications are on the current form")]
        public void ThenCurrentFormNotificationExist(Table formNotifications)
        {
            _crmContext.CommandProcessor.Execute(new AssertFormNotificationsCommand(_crmContext, _seleniumContext, null, formNotifications));
        }

        [Then(@"the following error message appears: '(.*)'")]
        public void ThenErrorAppears(string errorMessage)
        {
            _crmContext.CommandProcessor.Execute(new AssertErrorDialogCommand(_crmContext, _seleniumContext, errorMessage));
        }

        [Then("(.*)'s form has the following ribbon state")]
        public void ThenFormHasRibbonItems(string alias, Table table)
        {
            _crmContext.CommandProcessor.Execute(new AssertRibbonStateCommand(_crmContext, _seleniumContext, alias, table));
        }
    }
}
