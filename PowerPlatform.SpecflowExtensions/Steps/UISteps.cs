using BoDi;
using PowerPlatform.SpecflowExtensions.Commands;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Steps
{
    [Binding]
    public class UISteps
    {
        private ICrmContext _crmContext;
        private IObjectContainer _container;

        public UISteps(ICrmContext crmContext, IObjectContainer container)
        {
            _crmContext = crmContext;
            _container = container;
        }

        [Then(@"(.*) has the following form notifications")]
        public void ThenFormNotificationExist(string alias, Table formNotifications)
        {
            _crmContext.CommandProcessor.Execute(new AssertFormNotificationsCommand(_container, alias, formNotifications));
        }


        [Then(@"the following form notifications are on the current form")]
        public void ThenCurrentFormNotificationExist(Table formNotifications)
        {
            _crmContext.CommandProcessor.Execute(new AssertFormNotificationsCommand(_container, null, formNotifications));
        }

        [Then(@"the following error message appears: '(.*)'")]
        public void ThenErrorAppears(string errorMessage)
        {
            _crmContext.CommandProcessor.Execute(new AssertErrorDialogCommand(_container, errorMessage));
        }

        [Then("(.*)'s form has the following form state")]
        public void ThenFieldsAreVisibleOnForm(string alias, Table table)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _crmContext.TableConverter.ConvertTable(aliasRef.LogicalName, table);

            _crmContext.CommandProcessor.Execute(new AssertFormStateCommand(_container, aliasRef, table));

        }
    }
}
