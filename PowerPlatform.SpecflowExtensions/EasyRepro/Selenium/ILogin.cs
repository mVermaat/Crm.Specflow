using PowerPlatform.SpecflowExtensions.Connectivity;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    internal interface ILogin
    {
        void Login(ICrmConnection connection);
    }
}
