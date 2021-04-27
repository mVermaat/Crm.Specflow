using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro.Fields;
using Vermaat.Crm.Specflow.FormLoadConditions;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    class OpportunityCloseDialog
    {
        private readonly UCIApp _app;
        private readonly EntityMetadata _entityMetadata;
        private readonly bool _closeAsWon;

        private OpportunityCloseDialog(UCIApp app, EntityMetadata entityMetadata, bool closeAsWon) 
        {
            _app = app;
            _entityMetadata = entityMetadata;
            _closeAsWon = closeAsWon;
        }


        public static OpportunityCloseDialog CreateDialog(UCIApp app, FormData formData, bool closeAsWon)
        {
            OpenOpportunityCloseDialog(app.Client, formData, closeAsWon);

            var metadata = GlobalTestingContext.Metadata.GetEntityMetadata("opportunityclose");
            return new OpportunityCloseDialog(app, metadata, closeAsWon);
        }

        public void EnterData(CrmTestingContext crmContext, Table closeData)
        {
            foreach(var row in closeData.Rows)
            {
                var attribute = _entityMetadata.Attributes.FirstOrDefault(a => a.LogicalName == row[Constants.SpecFlow.TABLE_KEY]);
                if (attribute == null)
                    throw new TestExecutionException(Constants.ErrorCodes.ATTRIBUTE_DOESNT_EXIST, row[Constants.SpecFlow.TABLE_KEY], _entityMetadata.LogicalName);

                OpportunityCloseDialogField field = new OpportunityCloseDialogField(_app, attribute);
                field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
            }
        }

        public void FinishDialog()
        {
            _app.Client.Execute(BrowserOptionHelper.GetOptions($"Opening opportunity close dialog"), driver =>
            {
                    driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]),
                               new TimeSpan(0, 0, 5),
                               d => { driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok])); },
                               () => { throw new InvalidOperationException("The Close Opportunity dialog is not available."); });

                    HelperMethods.WaitForFormLoad(_app.WebDriver, new NoBusinessProcessError(), new RecordHasStatus(_closeAsWon ? "Won" : "Lost"));
                
                return true;
            });
        }
        private static bool OpenOpportunityCloseDialog(WebClient client, FormData formData, bool closeAsWon)
        {
            if(closeAsWon)
                formData.CommandBar.ClickButton("Close as Won");
            else
                formData.CommandBar.ClickButton("Close as Lost");

            return client.Execute(BrowserOptionHelper.GetOptions($"Opening opportunity close dialog"), driver =>
            {
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]));
                return true;
            });
        }
    }
}
