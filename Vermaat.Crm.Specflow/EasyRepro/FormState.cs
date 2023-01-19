using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public void CollapseHeader()
        {
            ExpandCollapseHeader(false);
        }


        public void ExpandHeader()
        {
            ExpandCollapseHeader(true);
        }

        private void ExpandCollapseHeader(bool expand)
        {
            Logger.WriteLine($"Expanding header: {expand}");
            var header = _app.WebDriver.WaitUntilClickable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Header, string.Empty));

            if (header == null)
                throw new InvalidOperationException("Form header unavailable");

            if (!bool.TryParse(header.GetAttribute("aria-expanded"), out var expanded) || expanded != expand)
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
