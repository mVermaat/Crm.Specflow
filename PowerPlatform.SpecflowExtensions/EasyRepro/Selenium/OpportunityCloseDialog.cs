using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using PowerPlatform.SpecflowExtensions.EasyRepro.Controls;
using PowerPlatform.SpecflowExtensions.EasyRepro.FormLoadConditions;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    class OpportunityCloseDialog
    {
        private readonly INavigation _navigation;
        private readonly SeleniumExecutor _executor;
        private readonly EntityMetadata _entityMetadata;
        private readonly bool _closeAsWon;

        private OpportunityCloseDialog(INavigation navigation, SeleniumExecutor executor, EntityMetadata entityMetadata, bool closeAsWon)
        {
            _navigation = navigation;
            _executor = executor;
            _entityMetadata = entityMetadata;
            _closeAsWon = closeAsWon;
        }


        public static OpportunityCloseDialog CreateDialog(INavigation navigation, SeleniumExecutor executor, bool closeAsWon)
        {
            OpenOpportunityCloseDialog(executor, closeAsWon);

            var metadata = GlobalContext.Metadata.GetEntityMetadata("opportunityclose");
            return new OpportunityCloseDialog(navigation, executor, metadata, closeAsWon);
        }

        public void EnterData(ICrmContext crmContext, Table closeData)
        {
            foreach (var row in closeData.Rows)
            {
                var attribute = _entityMetadata.Attributes.FirstOrDefault(a => a.LogicalName == row[Constants.SpecFlow.TABLE_KEY]);
                if (attribute == null)
                    throw new TestExecutionException(Constants.ErrorCodes.ATTRIBUTE_DOESNT_EXIST, row[Constants.SpecFlow.TABLE_KEY], _entityMetadata.LogicalName);

                OpportunityCloseDialogField field = new OpportunityCloseDialogField(attribute, attribute.LogicalName);
                field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
            }
        }

        public void FinishDialog()
        {
            _executor.Execute("Opening opportunity close dialog", driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]),
                           new TimeSpan(0, 0, 5),
                           d => { driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok])); },
                           () => { throw new InvalidOperationException("The Close Opportunity dialog is not available."); });

                _navigation.WaitForFormLoad(new NoBusinessProcessError(), new RecordHasStatus(_closeAsWon ? "Won" : "Lost"));

                return true;
            });
        }
        private static bool OpenOpportunityCloseDialog(SeleniumExecutor executor, bool closeAsWon)
        {
            return executor.Execute("Opening opportunity close dialog", driver =>
            {
                string xPathQuery = String.Empty;
                if (closeAsWon)
                    xPathQuery = AppElements.Xpath[AppReference.Entity.CloseOpportunityWin];
                else
                    xPathQuery = AppElements.Xpath[AppReference.Entity.CloseOpportunityLoss];

                var closeBtn = driver.WaitUntilAvailable(By.XPath(xPathQuery), "Opportunity Close Button is not available");
                closeBtn?.Click();
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]));

                return true;
            });
        }
    }
}
