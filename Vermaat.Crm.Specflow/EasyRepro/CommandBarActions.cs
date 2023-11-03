using Microsoft.Xrm.Sdk;
using Vermaat.Crm.Specflow.FormLoadConditions;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class CommandBarActions
    {
        private readonly UCIApp _app;

        public CommandBarActions(UCIApp app)
        {
            _app = app;
        }

        public void ClickButton(string buttonText)
        {
            SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateClickRibbonItemCommand(buttonText));
        }


        public bool IsButtonAvailable(string name)
            => SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateGetRibbonItemCommand(name)) != null;


        public void ActivateQuote()
        {
            Logger.WriteLine("Activating Quote");
            var record = new EntityReference("quote", _app.App.Entity.GetObjectId());
            ClickButton(_app.LocalizedTexts[Constants.LocalizedTexts.ActivateQuoteButton, _app.UILanguageCode]);
            HelperMethods.WaitForFormLoad(_app.WebDriver, new RecordHasStatus(record, 1)); // Active status

        }

        public EntityReference CreateOrder()
        {
            Logger.WriteLine("Creating Sales Order from Quote");
            ClickButton(_app.LocalizedTexts[Constants.LocalizedTexts.CreateOrderButton, _app.UILanguageCode]);
            CreateOrderDialog();

            return new EntityReference("salesorder", _app.App.Entity.GetObjectId());
        }

        public void Delete()
        {
            Logger.WriteLine($"Deleting record");
            ClickButton(_app.LocalizedTexts[Constants.LocalizedTexts.DeleteButton, _app.UILanguageCode]);
            _app.App.Dialogs.ConfirmationDialog(true);
        }

        public EntityReference ReviseQuote()
        {
            return SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateReviseQuoteCommand());
        }

        private void CreateOrderDialog()
        {
            SeleniumCommandProcessor.ExecuteCommand(_app, _app.SeleniumCommandFactory.CreateConvertActiveQuoteToSalesOrderCommand());
        }
    }
}
