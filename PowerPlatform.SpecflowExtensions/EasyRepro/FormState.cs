using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro
{
    internal class FormState
    {
        private readonly SeleniumExecutor _executor;

        public string CurrentTab { get; private set; }

        public FormState(SeleniumExecutor executor)
        {
            _executor = executor;
        }
       
        public void ExpandHeader()
        {
            _executor.Execute("Expand Header", (driver, selectors) =>
            {
                Logger.WriteLine("Expanding headers");
                var header = selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Header);
                driver.ClickWhenAvailable(header);
                return true;
            });
            
        }

        public void ExpandTab(string tabLabel)
        {
            _executor.Execute("Expand Tab", (driver, selectors, app) =>
            {
                if (string.IsNullOrEmpty(CurrentTab) || !CurrentTab.Equals(tabLabel, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.WriteLine($"Expanding tab {tabLabel}");
                    app.Entity.SelectTab(tabLabel);
                    CurrentTab = tabLabel;
                }
                return true;
            });

           

        }
    }
}
