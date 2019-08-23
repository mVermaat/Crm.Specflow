using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _app.App.CommandBar.ClickCommand(buttonText);
        }

        public void ActivateQuote()
        {
            Logger.WriteLine("Activating Quote");
            ClickButton(_app.ButtonTexts.ActivateQuote);
        }

        public EntityReference CreateOrder()
        {
            Logger.WriteLine("Creating Sales Order from Quote");
            ClickButton(_app.ButtonTexts.CreateOrder);
            CreateOrderDialog();

            return new EntityReference("salesorder", _app.App.Entity.GetObjectId());
        }

        public void Delete()
        {
            Logger.WriteLine($"Deleting record");
            ClickButton(_app.ButtonTexts.Delete);
            _app.App.Dialogs.ConfirmationDialog(true);
        }

        public EntityReference ReviseQuote()
        {
            Logger.WriteLine("Revising Quote");
            return _app.Client.Execute(BrowserOptionHelper.GetOptions($"Revise Quote"), driver =>
            {
                ClickButton(_app.ButtonTexts.ReviseQuote);

                _app.Client.Browser.ThinkTime(1000);
                HelperMethods.WaitForFormLoad(driver);

                return new EntityReference("quote", _app.App.Entity.GetObjectId()); ;
            }).Value;            
        }


        private void CreateOrderDialog()
        {
            _app.Client.Execute(BrowserOptionHelper.GetOptions($"Create Sales Order"), driver =>
            {
                
                var container = driver.WaitUntilAvailable(By.XPath(Constants.XPath.DIALOG_CONTAINER));
                var button = container.FindElement(By.XPath(Constants.XPath.DIALOG_OK));

                button.Click();
                _app.Client.Browser.ThinkTime(2000);
                HelperMethods.WaitForFormLoad(driver);

                return true;
            });
        }
    }
}
