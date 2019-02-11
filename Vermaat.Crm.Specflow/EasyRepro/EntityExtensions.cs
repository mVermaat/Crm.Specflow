using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public static class EntityExtensions
    {
        public static BrowserCommandResult<bool> IsElementVisible(this Entity entity, string field)
        {
            return entity.Execute($"Is Field visible: {field}", driver =>
            {
                return driver.WaitUntilVisible(By.Id(field), TimeSpan.FromSeconds(5));
            });
        }        
    }
}
