using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
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

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium.Entity
{
    class OpportunityActions : IOpportunityActions
    {
        private const string _logicalName = "opportunity";
        private BrowserSession _session;
        private SeleniumExecutor _executor;
        

        public void CloseOpportunity(ICrmContext crmContext, bool closeAsWon, Table closeData)
        {
            _executor.Execute("closing opportunity", 
                (driver, selectors) =>
            {
                string xPathQuery = string.Empty;
                if (closeAsWon)
                    xPathQuery = AppElements.Xpath[AppReference.Entity.CloseOpportunityWin];
                else
                    xPathQuery = AppElements.Xpath[AppReference.Entity.CloseOpportunityLoss];

                var closeBtn = driver.WaitUntilAvailable(By.XPath(xPathQuery), "Opportunity Close Button is not available");
                closeBtn?.Click();
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]));

                foreach (var row in closeData.Rows)
                {
                    var attribute = GlobalContext.Metadata.GetAttributeMetadata(_logicalName, row[Constants.SpecFlow.TABLE_KEY]);
                    if (attribute == null)
                        throw new TestExecutionException(Constants.ErrorCodes.ATTRIBUTE_DOESNT_EXIST, row[Constants.SpecFlow.TABLE_KEY], _logicalName);

                    OpportunityCloseDialogField field = new OpportunityCloseDialogField(attribute, attribute.LogicalName);
                    field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);

                    driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]),
                          new TimeSpan(0, 0, 5),
                          d => { driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok])); },
                          () => { throw new InvalidOperationException("The Close Opportunity dialog is not available."); });
                    _session.WaitForFormLoad(new NoBusinessProcessError(), new RecordHasStatus(closeAsWon ? "Won" : "Lost"));

                }
            });
        }

        public void Initialize(BrowserSession session, SeleniumExecutor executor)
        {
            _session = session;
            _executor = executor;
        }

    }
}
