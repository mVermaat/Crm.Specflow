using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
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
        IReadOnlyCollection<FormNotification> GetFormNotifications();
        void FillForm(ICrmContext crmContext, Table tableWithDefaults);
        void Save(bool saveIfDuplicate);
        Guid GetRecordId();
    }
}
