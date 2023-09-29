using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.FormLoadConditions;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    internal class ReviseQuoteCommand : ISeleniumCommandFunc<EntityReference>
    {

        public CommandResult<EntityReference> Execute(BrowserInteraction browserInteraction)
        {
            var currentRecord = new EntityReference("quote", SeleniumCommandProcessor.ExecuteCommand(browserInteraction, browserInteraction.SeleniumCommandFactory.CreateGetCurrentRecordIdCommand()));

            SeleniumCommandProcessor.ExecuteCommand(browserInteraction, browserInteraction.SeleniumCommandFactory
                .CreateClickRibbonItemCommand(browserInteraction.LocalizedTexts[Constants.LocalizedTexts.ReviseQuoteButton, browserInteraction.UiLanguageCode]));

            HelperMethods.WaitForFormLoad(browserInteraction.Driver, new RecordHasStatus(currentRecord, 1));
            currentRecord = new EntityReference("quote", SeleniumCommandProcessor.ExecuteCommand(browserInteraction, browserInteraction.SeleniumCommandFactory.CreateGetCurrentRecordIdCommand()));

            return CommandResult<EntityReference>.Success(currentRecord);
        }
    }
}
