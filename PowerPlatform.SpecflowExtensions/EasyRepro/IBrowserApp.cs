using BoDi;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro
{
    public interface IBrowserApp : IDisposable
    {
        void Initialize(WebClient client, ISeleniumExecutor executor);
        void Refresh(IObjectContainer container);
    }
}
