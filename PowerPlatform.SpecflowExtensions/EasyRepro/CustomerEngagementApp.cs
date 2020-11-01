using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro
{
    class CustomerEngagementApp : IBrowserApp
    {
        private SeleniumExecutor _executor;

        public void Initialize(SeleniumExecutor executor)
        {
            _executor = executor;
        }
    }
}
