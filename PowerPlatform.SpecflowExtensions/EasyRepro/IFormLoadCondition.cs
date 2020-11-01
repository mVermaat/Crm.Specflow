using OpenQA.Selenium;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro
{
    public interface IFormLoadCondition
    {
        bool Evaluate(IWebDriver driver, SeleniumSelectorData selectors);
    }
}
