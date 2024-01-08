using OpenQA.Selenium;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    static class WebElementExtensions
    {
        public static bool TryFindElement(this IWebElement element, By by, out IWebElement foundElement)
        {
            try
            {
                foundElement = element.FindElement(by);
                return foundElement != null;
            }
            catch (NoSuchElementException)
            {
                foundElement = null;
                return false;
            }
        }
    }
}
