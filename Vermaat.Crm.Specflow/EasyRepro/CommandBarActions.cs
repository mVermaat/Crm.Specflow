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

        public void ActivateQuote()
        {
            Logger.WriteLine("Activating Quote");
            _app.App.CommandBar.ClickCommand(_app.ButtonTexts.ActivateQuote);
        }

        public EntityReference CreateOrder()
        {
            Logger.WriteLine("Creating Sales Order from Quote");
            _app.App.CommandBar.ClickCommand(_app.ButtonTexts.CreateOrder);
            CreateOrderDialog();

            return new EntityReference("salesorder", _app.App.Entity.GetObjectId());
        }

        public void Delete()
        {
            Logger.WriteLine($"Deleting record");
            _app.App.CommandBar.ClickCommand(_app.ButtonTexts.Delete);
            _app.App.Dialogs.ConfirmationDialog(true);
        }

        public void ReviseQuote()
        {
            Logger.WriteLine("Revising Quote");
            _app.App.CommandBar.ClickCommand(_app.ButtonTexts.ReviseQuote);
        }


        private void CreateOrderDialog()
        {
            _app.Client.Execute(BrowserOptionHelper.GetOptions($"Create Sales Order"), driver =>
            {
                
                var container = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Constants.EasyReproReference.DIALOG_CONTAINER]));
                var button = container.FindElement(By.XPath(Elements.Xpath[Constants.EasyReproReference.DIALOG_OK]));

                button.Click();
                _app.Client.Browser.ThinkTime(1000);
                driver.WaitForPageToLoad();
                driver.WaitUntilClickable(By.XPath(Elements.Xpath[Reference.Entity.Form]),
                    new TimeSpan(0, 0, 30),
                    null,
                    d => { throw new Exception("CRM Record is Unavailable or not finished loading. Timeout Exceeded"); }
                );
                return true;
            });
        }
    }
}
