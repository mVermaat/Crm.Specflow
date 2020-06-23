using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Interfaces
{
    public interface ISeleniumContext
    {
        BrowserOptions BrowserOptions { get; }
    }
}
