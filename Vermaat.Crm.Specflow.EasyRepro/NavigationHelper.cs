using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    static class NavigationHelper
    {
        public static void OpenNewForm(Browser browser, string entityName)
        {
            var currentUri = new Uri(browser.Driver.Url);
            var newRecordFormUri = new Uri($"{currentUri.Scheme}://{currentUri.Authority}/main.aspx?etn={entityName}&pagetype=entityrecord");
            browser.Entity.OpenEntity(newRecordFormUri);
        }

        public static void OpenRecord(Browser browser, EntityReference crmRecord)
        {
            browser.Entity.OpenEntity(crmRecord.LogicalName, crmRecord.Id);
        }
    }
}
