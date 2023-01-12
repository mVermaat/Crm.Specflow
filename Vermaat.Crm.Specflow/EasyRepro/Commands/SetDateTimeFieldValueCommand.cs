using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class SetDateTimeFieldValueCommand : ISeleniumCommand
    {
        private readonly string _logicalName;
        private readonly DateTime? _value;
        private readonly bool _dateOnly;
        private readonly string _formatDate;
        private readonly string _formatTime;

        public SetDateTimeFieldValueCommand(string logicalName, DateTime? value, bool dateOnly,
            string formatDate, string formatTime)
        {
            _logicalName = logicalName;
            _value = value;
            _dateOnly = dateOnly;
            _formatDate = formatDate;
            _formatTime = formatTime;
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            browserInteraction.Driver.WaitForTransaction();
            IWebElement container = GetFieldContainer(browserInteraction, _logicalName);

            var dateField = container.WaitUntilAvailable(
                SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_DateTime_Time_Input),
                $"Input for {_logicalName} does not exist");
            try
            {
                var date = _value.HasValue ? _formatDate == null ? _value.Value.ToShortDateString() : _value.Value.ToString(_formatDate) : string.Empty;
                browserInteraction.Driver.RepeatUntil(() =>
                {
                    ClearFieldValue(dateField);
                    dateField.SendKeys(date);
                },
                    d => dateField.GetAttribute("value") == date,
                    new TimeSpan(0, 0, 9), 3
                );
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {_value}. Actual: {dateField.GetAttribute("value")}", ex);
            }

            // date only fields don't have a time control
            // clearing the date part of a datetime field is enough to clear both
            if (_dateOnly || !_value.HasValue)
                return CommandResult.Success();

            // Time field becomes visible after focus is lost.
            // Clearfocus will have unwanted side effects like popups or redirects for some reason.
            dateField.SendKeys(Keys.Tab);
            browserInteraction.Driver.WaitForTransaction();

            var timeFieldXPath = By.XPath($"//div[contains(@data-id,'{_logicalName}.fieldControl._timecontrol-datetime-container')]/div/div/input");
            var timeField = browserInteraction.Driver.WaitUntilAvailable(timeFieldXPath, TimeSpan.FromSeconds(5), "Time control of datetime field not available");
            try
            {
                var time = _value.HasValue ? _formatTime == null ? _value.Value.ToShortTimeString() : _value.Value.ToString(_formatTime) : string.Empty;
                browserInteraction.Driver.RepeatUntil(() =>
                {
                    ClearFieldValue(timeField);
                    timeField.SendKeys(time + Keys.Tab);
                },
                    d => timeField.GetAttribute("value") == time,
                    new TimeSpan(0, 0, 9), 3
                );
                browserInteraction.Driver.WaitForTransaction();

            }
            catch (WebDriverTimeoutException ex)
            {
                throw new InvalidOperationException($"Timeout after 10 seconds. Expected: {_value}. Actual: {timeField.GetAttribute("value")}", ex);
            }

            return CommandResult.Success();
        }

        private static void ClearFieldValue(IWebElement field)
        {
            if (field.GetAttribute("value").Length > 0)
            {
                field.SendKeys(Keys.Control + "a");
                field.SendKeys(Keys.Backspace);
            }
            Thread.Sleep(2000);
        }

        protected virtual IWebElement GetFieldContainer(BrowserInteraction browserInteraction, string logicalName)
        {
            return browserInteraction.Driver.WaitUntilAvailable(
                    SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_DateContainer, logicalName),
                    $"Field: {logicalName} does not exist");
        }
    }
}
