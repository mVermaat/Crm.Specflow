using BoDi;
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
    public class OpportunityActions : IBrowserApp
    {
        private const string _logicalName = "opportunityclose";
        private ISeleniumExecutor _executor;

        public void CloseOpportunity(ICrmContext crmContext, INavigation navigation, bool closeAsWon, Table closeData)
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

                    OpportunityCloseDialogField field = new OpportunityCloseDialogField(_executor, attribute, attribute.LogicalName);
                    field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
                }

                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]),
                          new TimeSpan(0, 0, 5),
                          d => { driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok])); },
                          () => { throw new InvalidOperationException("The Close Opportunity dialog is not available."); });
                navigation.WaitForFormLoad(new NoBusinessProcessError(), new RecordHasStatus(closeAsWon ? "Won" : "Lost"));

                return true;
            });
        }

        public void Dispose()
        {
        }

        public void Initialize(WebClient client, ISeleniumExecutor executor)
        {
            _executor = executor;
        }

        public void Refresh(IObjectContainer container)
        {
            
        }
    }
}
