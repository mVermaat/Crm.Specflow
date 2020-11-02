using BoDi;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using PowerPlatform.SpecflowExtensions.EasyRepro.FormLoadConditions;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Apps
{
    public class QuoteActions : IBrowserApp
    {
        private ISeleniumExecutor _executor;


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

        public void ActivateQuote()
        {
            Logger.WriteLine("Activating Quote");
            _executor.Execute("Activate Quote",
                (driver, selectors, app) =>
                {
                    app.CommandBar.ClickCommand("Activate Quote");
                    return true;
                });
        }

        public EntityReference CreateOrder(INavigation navigation)
        {
            Logger.WriteLine("Creating Sales Order from Quote");
            return _executor.Execute("Revise Quote",
               (driver, selectors, app) =>
               {
                   app.CommandBar.ClickCommand("Create Order");

                   var container = driver.WaitUntilAvailable(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Container));
                   var button = container.FindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_OK));

                   button.Click();
                   navigation.WaitForFormLoad(new FormIsOfEntity("salesorder"));

                   return new EntityReference("salesorder", app.Entity.GetObjectId());
               });
        }

        public EntityReference ReviseQuote(INavigation navigation)
        {
            Logger.WriteLine("Revising Quote");
            return _executor.Execute("Revise Quote",
                (driver, selectors, app) =>
            {
                app.CommandBar.ClickCommand("Revise");

                // Todo: XPath
                Thread.Sleep(1000);
                navigation.WaitForFormLoad();

                return new EntityReference("quote", app.Entity.GetObjectId());
            });
        }
    }
}
