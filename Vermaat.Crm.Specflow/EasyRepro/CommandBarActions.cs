using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _app.App.CommandBar.ClickCommand(buttonText);
        }


        public bool IsButtonAvailable(string name)
        {
            return _app.Client.Execute(BrowserOptionHelper.GetOptions($"Check ribbon button"), driver =>
            {
                IWebElement ribbon = null;

                //Find the button in the CommandBar
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container])))
                    ribbon = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Container]));

                if (ribbon == null)
                {
                    if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid])))
                        ribbon = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.ContainerGrid]));
                    else
                        throw new InvalidOperationException("Unable to find the ribbon.");
                }

                //Get the CommandBar buttons
                var items = ribbon.FindElements(By.TagName("li"));

                //Is the button in the ribbon?
                if (items.Any(x => x.GetAttribute("aria-label").Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
                else
                {
                    //Is the button in More Commands?
                    if (items.Any(x => x.GetAttribute("aria-label").Equals("More Commands", StringComparison.OrdinalIgnoreCase)))
                    {
                        //Click More Commands
                        items.FirstOrDefault(x => x.GetAttribute("aria-label").Equals("More Commands", StringComparison.OrdinalIgnoreCase)).Click(true);
                        driver.WaitForTransaction();

                        //Find the button
                        return driver.HasElement(By.XPath(AppElements.Xpath[AppReference.CommandBar.Button].Replace("[NAME]", name)));
                    }
                    else
                        return false;
                }
            });
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
                
                var container = driver.WaitUntilAvailable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Container));
                var button = container.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_OK));

                button.Click();
                HelperMethods.WaitForFormLoad(driver, new FormIsOfEntity("salesorder"));

                return true;
            });
        }
    }
}
