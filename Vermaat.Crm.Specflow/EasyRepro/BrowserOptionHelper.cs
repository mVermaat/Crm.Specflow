using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public static class BrowserOptionHelper
    {
        public static BrowserCommandOptions GetOptions(string commandName)
        {
            return new BrowserCommandOptions(Microsoft.Dynamics365.UIAutomation.Browser.Constants.DefaultTraceSource,
                commandName,
                0,
                0,
                null,
                true,
                typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }
    }
}
