using BoDi;
using FluentAssertions;
using PowerPlatform.SpecflowExtensions.EasyRepro.Apps;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class AssertFormNotificationsCommand : BrowserOnlyCommand
    {
        private readonly string _alias;
        private readonly Table _notificationTable;

        public AssertFormNotificationsCommand(IObjectContainer container, string alias,
            Table notificationTable)
            : base(container)
        {
            _alias = alias;
            _notificationTable = notificationTable;
        }

        public override void Execute()
        {
            var form = GetForm();

            var notifications = form.GetFormNotifications();
            Logger.WriteLine($"Found {notifications.Count} notifications");

            _notificationTable.RowCount.Should().Be(notifications.Count,
                $"Different amount of form notifications: Actual form notifications: {string.Join(", ", notifications.Select(n => n.Message))}");

            Logger.WriteLine("Converting expected notification to Dictionary with message (tolower) as key");
            var expectedNotifications = _notificationTable.Rows.ToDictionary(r => r[Constants.SpecFlow.TABLE_FORMNOTIFICATION_MESSAGE].ToLower());

            foreach (var notification in notifications)
            {
                expectedNotifications.TryGetValue(notification.Message.ToLower(), out TableRow row)
                    .Should().BeTrue($"Notification {notification.Message} wasn't expected");
                notification.Type.ToString().ToLower().Should().Be(row[Constants.SpecFlow.TABLE_FORMNOTIFICATION_LEVEL]?.ToLower()
                    , $"Notification {notification.Message} has a wrong level");
            }

        }

        private IForm GetForm()
        {
            var app = GlobalContext.ConnectionManager.GetCurrentBrowserSession(_seleniumContext)
                .GetApp<CustomerEngagementApp>(_container);

            if(string.IsNullOrEmpty(_alias))
            {
                return app.LastForm;
            }
            else
            {
                var aliasRef = _crmContext.RecordCache.Get(_alias);
                return app.GetForm(aliasRef.LogicalName);
            }

            
        }
    }
}
