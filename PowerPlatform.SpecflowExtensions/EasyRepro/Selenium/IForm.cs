using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    public interface IForm
    {
        void FillForm(ICrmContext crmContext, Table tableWithDefaults);
    }
}
