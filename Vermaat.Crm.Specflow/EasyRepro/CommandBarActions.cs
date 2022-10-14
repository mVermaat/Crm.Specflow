using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro.Commands;
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
            SeleniumCommandProcessor.ExecuteCommand(_app, new ClickRibbonItemCommand(buttonText));
        }


        public bool IsButtonAvailable(string name)
            => SeleniumCommandProcessor.ExecuteCommand(_app, new GetRibbonItemCommand(name)) != null;
        

        public void ActivateQuote()
        {
            Logger.WriteLine("Activating Quote");
            ClickButton(_app.LocalizedTexts[Constants.LocalizedTexts.ActivateQuoteButton, _app.UILanguageCode]);
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
            Logger.WriteLine("Revising Quote");
            return _app.Client.Execute(BrowserOptionHelper.GetOptions($"Revise Quote"), driver =>
            {
                ClickButton(_app.LocalizedTexts[Constants.LocalizedTexts.ReviseQuoteButton, _app.UILanguageCode]);

                _app.Client.Browser.ThinkTime(1000);
                HelperMethods.WaitForFormLoad(driver);

                return new EntityReference("quote", _app.App.Entity.GetObjectId()); ;
            }).Value;            
        }

        private void CreateOrderDialog()
        {
            _app.Client.Execute(BrowserOptionHelper.GetOptions($"Create Sales Order"), driver =>
            {
                
                var container = driver.WaitUntilAvailable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Container));
                var button = container.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_OK));

                button.Click();
                HelperMethods.WaitForFormLoad(driver, new FormIsOfEntity("salesorder"));

                return true;
            });
        }
    }
}
