using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            catch(NoSuchElementException)
            {
                foundElement = null;
                return false;
            }
        }
    }
}
