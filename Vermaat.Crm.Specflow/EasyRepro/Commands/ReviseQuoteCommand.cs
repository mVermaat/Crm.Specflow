using Microsoft.Xrm.Sdk;
using Vermaat.Crm.Specflow.FormLoadConditions;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class ReviseQuoteCommand : ISeleniumCommandFunc<EntityReference>
    {

        public CommandResult<EntityReference> Execute(BrowserInteraction browserInteraction)
        {
            Logger.WriteLine("Revising Quote");
            var currentRecord = new EntityReference("quote", SeleniumCommandProcessor.ExecuteCommand(browserInteraction, browserInteraction.SeleniumCommandFactory.CreateGetCurrentRecordIdCommand()));

            SeleniumCommandProcessor.ExecuteCommand(browserInteraction, browserInteraction.SeleniumCommandFactory
                .CreateClickRibbonItemCommand(browserInteraction.LocalizedTexts[Constants.LocalizedTexts.ReviseQuoteButton, browserInteraction.UiLanguageCode]));

            HelperMethods.WaitForFormLoad(browserInteraction.Driver, new RecordHasStatus(currentRecord, 1));
            currentRecord = new EntityReference("quote", SeleniumCommandProcessor.ExecuteCommand(browserInteraction, browserInteraction.SeleniumCommandFactory.CreateGetCurrentRecordIdCommand()));

            return CommandResult<EntityReference>.Success(currentRecord);
        }
    }
}
