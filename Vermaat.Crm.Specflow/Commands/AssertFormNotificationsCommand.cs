using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    public class AssertFormNotificationsCommand : BrowserOnlyCommand
    {
        private readonly string _alias;
        private readonly Table _notificationTable;

        public AssertFormNotificationsCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, string alias,
            Table notificationTable) 
            : base(crmContext, seleniumContext)
        {
            _alias = alias;
            _notificationTable = notificationTable;
        }

        public override void Execute()
        {
            var form = GetFormData();

            
            var notifications = form.GetFormNotifications();
            Logger.WriteLine($"Found {notifications.Count} notifications");

            Assert.AreEqual(_notificationTable.RowCount, notifications.Count,
                $"Different amount of form notifications: Actual form notifications: {string.Join(", ", notifications.Select(n => n.Message))}");

            Logger.WriteLine("Converting expected notification to Dictionary with message (tolower) as key");
            var expectedNotifications = _notificationTable.Rows.ToDictionary(r => r[Constants.SpecFlow.TABLE_FORMNOTIFICATION_MESSAGE].ToLower());

            foreach(var notification in notifications)
            {
                Assert.IsTrue(expectedNotifications.TryGetValue(notification.Message.ToLower(), out TableRow row), $"Notification {notification.Message} wasn't expected");
                Assert.AreEqual(notification.Type.ToString().ToLower(), row[Constants.SpecFlow.TABLE_FORMNOTIFICATION_LEVEL]?.ToLower(), $"Notification {notification.Message} has a wrong level");
            }

        }

        private FormData GetFormData()
        {
            if(string.IsNullOrEmpty(_alias))
            {
                return _seleniumContext.GetBrowser().LastFormData;
            }
            else
            {
                var aliasRef = _crmContext.RecordCache.Get(_alias, true);
                return _seleniumContext.GetBrowser().OpenRecord(new OpenFormOptions(aliasRef));
            }
        }
    }
}
