using BoDi;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //_app.App.CommandBar.ClickCommand(buttonText);
            //ClickButton(_app.ButtonTexts.ActivateQuote);
        }
    }
}
