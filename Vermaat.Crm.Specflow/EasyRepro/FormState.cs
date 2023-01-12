using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormState
    {
        private readonly UCIApp _app;

        public string CurrentTab { get; set; }

        public FormState(UCIApp app)
        {
            _app = app;
        }

        public void ResetState()
        {
            CurrentTab = null;
        }


        public void ExpandHeader()
        {
            Logger.WriteLine("Expanding headers");
            var header = _app.WebDriver.WaitUntilClickable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Header, string.Empty));

            if (header == null)
                throw new InvalidOperationException("Form header unavailable");

            if (!bool.TryParse(header.GetAttribute("aria-expanded"), out var expanded) || !expanded)
                header.Click();           
        }

        public void ExpandTab(string tabLabel)
        {
            if (string.IsNullOrEmpty(CurrentTab) || !CurrentTab.Equals(tabLabel, StringComparison.OrdinalIgnoreCase))
            {
                Logger.WriteLine($"Expanding tab {tabLabel}");
                _app.App.Entity.SelectTab(tabLabel);
                CurrentTab = tabLabel;
            }
            
        }
    }
}
