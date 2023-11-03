using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

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
            var header = _app.WebDriver.WaitUntilClickable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Header, string.Empty), TimeSpan.FromSeconds(1));

            if (header == null)
            {
                if (expand)
                    throw new InvalidOperationException("Form header unavailable");
                else // If a form doesn't have any header fields, then the header will be null.
                     // Trying to collapse a header that doesn't exist is no problem
                    return;
            }

            if (!bool.TryParse(header.GetAttribute("aria-expanded"), out var expanded) || expanded != expand)
                header.Click();
        }

        public void ExpandTab(string tabLabel)
        {
            if (string.IsNullOrEmpty(CurrentTab) || !CurrentTab.Equals(tabLabel, StringComparison.OrdinalIgnoreCase))
            {
                Logger.WriteLine($"Expanding tab {tabLabel}");

                // if you want to expand a tab, it's possible the header is open and overlaps the tab you want to select, so collapse the header just in case.
                CollapseHeader();
                _app.App.Entity.SelectTab(tabLabel);
                CurrentTab = tabLabel;
            }

        }
    }
}
