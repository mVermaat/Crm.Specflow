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
                SelectTab(tabLabel);
                CurrentTab = tabLabel;
            }
            
        }

        private void SelectTab(string tabName)
        {
            Thread.Sleep(2000);

            _app.Client.Execute($"Select Tab", driver =>
            {
                IWebElement tabList;
                if (driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.DialogContext])))
                {
                    var dialogContainer = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Dialogs.DialogContext]));
                    tabList = dialogContainer.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TabList]));
                }
                else
                {
                    tabList = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TabList]));
                }

                ClickTab(driver, tabList, AppElements.Xpath[AppReference.Entity.Tab], tabName);

                driver.WaitForTransaction();
                return true;
            });
        }

        private void ClickTab(IWebDriver driver, IWebElement tabList, string xpath, string name)
        {
            IWebElement moreTabsButton;
            IWebElement listItem;
            // Look for the tab in the tab list, else in the more tabs menu
            IWebElement searchScope = null;
            if (tabList.HasElement(By.XPath(string.Format(xpath, name))))
            {
                searchScope = tabList;
            }
            else if (tabList.TryFindElement(By.XPath(AppElements.Xpath[AppReference.Entity.MoreTabs]), out moreTabsButton))
            {
                moreTabsButton.Click();

                // No tab to click - subtabs under 'Related' are automatically expanded in overflow menu
                if (name == "Related")
                {
                    return;
                }
                else
                {
                    searchScope = driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.MoreTabsMenu]));
                }
            }

            if (searchScope.TryFindElement(By.XPath(string.Format(xpath, name)), out listItem))
            {
                listItem.Click(true);
            }
            else
            {
                throw new Exception($"The tab with name: {name} does not exist");
            }
        }
    }
}
