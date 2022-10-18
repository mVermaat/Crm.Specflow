using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class BrowserInteraction
    {
        public BrowserInteraction(IWebDriver driver, SeleniumSelectorData seleniumSelectorData,
            SeleniumCommandFactory seleniumCommandFactory, LocalizedTexts localizedTexts, int uiLanguageCode)
        {
            Driver = driver;
            Selectors = seleniumSelectorData;
            SeleniumCommandFactory = seleniumCommandFactory;
            LocalizedTexts = localizedTexts;
            UiLanguageCode = uiLanguageCode;
        }

        public IWebDriver Driver { get; }
        public LocalizedTexts LocalizedTexts { get; }
        public SeleniumSelectorData Selectors { get; }
        public SeleniumCommandFactory SeleniumCommandFactory { get; }

        public int UiLanguageCode { get; }

        public WebDriverWait GetWaitObject(TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(Driver, timeout ?? TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return wait;
        }
    }
}
