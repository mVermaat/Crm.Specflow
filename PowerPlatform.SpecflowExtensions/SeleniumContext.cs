using Microsoft.Dynamics365.UIAutomation.Browser;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions
{
    public class SeleniumContext : ISeleniumContext
    {
        public BrowserOptions BrowserOptions { get; set; }

        public SeleniumContext()
        {
            BrowserOptions = new BrowserOptions()
            {
                CleanSession = true,
                DriversPath = null,
                StartMaximized = true,
                PrivateMode = true,
                UCITestMode = true,
            };
        }
    }
}
