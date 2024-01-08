using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Linq;
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
            OpenOpportunityCloseDialog(app, formData, closeAsWon);

            var metadata = GlobalTestingContext.Metadata.GetEntityMetadata("opportunityclose");
            return new OpportunityCloseDialog(app, metadata, closeAsWon);
        }

        public void EnterData(CrmTestingContext crmContext, Table closeData)
        {
            foreach (var row in closeData.Rows)
            {
                var attribute = _entityMetadata.Attributes.FirstOrDefault(a => a.LogicalName == row[Constants.SpecFlow.TABLE_KEY]);
                if (attribute == null)
                    throw new TestExecutionException(Constants.ErrorCodes.ATTRIBUTE_DOESNT_EXIST, row[Constants.SpecFlow.TABLE_KEY], _entityMetadata.LogicalName);

                OpportunityCloseDialogField field = new OpportunityCloseDialogField(_app, attribute);
                field.SetValue(crmContext, row[Constants.SpecFlow.TABLE_VALUE]);
            }
        }

        public void FinishDialog(EntityReference opportunity)
        {
            _app.Client.Execute(BrowserOptionHelper.GetOptions($"Opening opportunity close dialog"), driver =>
            {
                driver.WaitUntilClickable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]),
                           new TimeSpan(0, 0, 5),
                           d => { driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok])); },
                           () => { throw new InvalidOperationException("The Close Opportunity dialog is not available."); });

                HelperMethods.WaitForFormLoad(_app.WebDriver, new NoBusinessProcessError(), new RecordHasStatus(opportunity, _closeAsWon ? 1 : 2));

                return true;
            });
        }
        private static bool OpenOpportunityCloseDialog(UCIApp app, FormData formData, bool closeAsWon)
        {
            if (closeAsWon)
                formData.CommandBar.ClickButton(app.LocalizedTexts[Constants.LocalizedTexts.CloseAsWon, app.UILanguageCode]);
            else
                formData.CommandBar.ClickButton(app.LocalizedTexts[Constants.LocalizedTexts.CloseAsLost, app.UILanguageCode]);

            return app.Client.Execute(BrowserOptionHelper.GetOptions($"Opening opportunity close dialog"), driver =>
            {
                driver.WaitUntilVisible(By.XPath(AppElements.Xpath[AppReference.Dialogs.CloseOpportunity.Ok]));
                return true;
            });
        }
    }
}
