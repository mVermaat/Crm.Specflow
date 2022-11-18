using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class SaveRecordCommand : ISeleniumCommand
    {
        private bool _saveIfDuplicate;

        public SaveRecordCommand(bool saveIfDuplicate)
        {
            _saveIfDuplicate = saveIfDuplicate;
        }

        private enum SaveStatus { Unsaved, Saving, Saved, Unknown }
        private enum SaveAction { DuplicateDetection, OK, Timeout }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            Logger.WriteLine($"Checking save status");
            

            DateTime timeout = DateTime.Now.AddSeconds(3);
            var status = GetSaveStatus(browserInteraction);

            while (status != SaveStatus.Unsaved && DateTime.Now < timeout)
            {
                Thread.Sleep(250);
                status = GetSaveStatus(browserInteraction);
            }

            switch (status)
            {
                case SaveStatus.Unsaved:
                    return Save(browserInteraction);
                case SaveStatus.Saved:
                    Logger.WriteLine("No save required");
                    return CommandResult.Success();
                case SaveStatus.Saving:
                    return CommandResult.Fail(true, Constants.ErrorCodes.FORM_SAVE_FAILED, "Already saving");
                case SaveStatus.Unknown:
                    return CommandResult.Fail(true, Constants.ErrorCodes.FORM_SAVE_FAILED, "Unknown Save Status");
                default:
                    throw new NotImplementedException($"Save status {status} not implemented");
            }
        }

        private CommandResult Save(BrowserInteraction browserInteraction)
        {
            Logger.WriteLine($"Saving record");

            SeleniumCommandProcessor.ExecuteCommand(browserInteraction, browserInteraction.SeleniumCommandFactory.CreateClickRibbonItemCommand(browserInteraction.LocalizedTexts[Constants.LocalizedTexts.SaveButton, browserInteraction.UiLanguageCode]));

            var timeout = DateTime.Now.AddSeconds(20);
            var unsavedTimeout = DateTime.Now.AddSeconds(10);
            bool saveCompleted = false;
            while (!saveCompleted && DateTime.Now < timeout)
            {
                Thread.Sleep(200);
                var saveStatus = GetSaveStatus(browserInteraction);

                if (saveStatus == SaveStatus.Saved)
                    saveCompleted = true;
                else if (saveStatus == SaveStatus.Unsaved)
                {
                    if (HasDuplicateDetection(browserInteraction))
                    {
                        if (_saveIfDuplicate)
                            AcceptDuplicateDetection(browserInteraction.Driver);
                        else
                            return CommandResult.Fail(false, Constants.ErrorCodes.FORM_SAVE_FAILED, $"Duplicate detected and saving with duplicate is not allowed");
                    }
                    else if (browserInteraction.Driver.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_ErrorDialog), out var errorDialog))
                    {
                        var errorDetails = errorDialog.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Subtitle));
                        if (!string.IsNullOrEmpty(errorDetails.Text))
                            return CommandResult.Fail(false, Constants.ErrorCodes.FORM_SAVE_FAILED, errorDetails.Text);
                    }
                    else if(DateTime.Now > unsavedTimeout)
                    {
                        var formNotifications = SeleniumCommandProcessor.ExecuteCommand(browserInteraction, browserInteraction.SeleniumCommandFactory.CreateGetFormNotificationsCommand());
                        return CommandResult.Fail(false, Constants.ErrorCodes.FORM_SAVE_FAILED, $"Detected Unsaved changes. Form Notifications: {string.Join(", ", formNotifications)}");
                    }
                } 
            }

            if (!saveCompleted)
                return CommandResult.Fail(false, Constants.ErrorCodes.FORM_SAVE_TIMEOUT, 20);

            Logger.WriteLine("Save sucessfull");
            return CommandResult.Success();
        }

        private bool HasDuplicateDetection(BrowserInteraction browserInteraction)
        {
            return browserInteraction.Driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionWindowMarker])) 
                && browserInteraction.Driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows]));
        }

        private void AcceptDuplicateDetection(IWebDriver driver)
        {
            Logger.WriteLine("Accepting duplicate");
            //Select the first record in the grid
            driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows]))[0].Click(true);

            //Click Ignore and Save
            driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton])).Click(true);
            driver.WaitForTransaction();
        }

        private SaveStatus GetSaveStatus(BrowserInteraction browserInteraction)
        {
            var saveStatusText = browserInteraction.Driver.WaitUntilAvailable(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SaveStatus), TimeSpan.FromSeconds(2))?.Text;

            if (string.IsNullOrEmpty(saveStatusText))
                return SaveStatus.Unknown;
            else if (saveStatusText.Equals($"- {browserInteraction.LocalizedTexts[Constants.LocalizedTexts.SaveStatusUnsaved, browserInteraction.UiLanguageCode]}", StringComparison.OrdinalIgnoreCase))
                return SaveStatus.Unsaved;
            else if (saveStatusText.Equals($"- {browserInteraction.LocalizedTexts[Constants.LocalizedTexts.SaveStatusSaving, browserInteraction.UiLanguageCode]}", StringComparison.OrdinalIgnoreCase))
                return SaveStatus.Saving;
            else if (saveStatusText.Equals($"- {browserInteraction.LocalizedTexts[Constants.LocalizedTexts.SaveStatusSaved, browserInteraction.UiLanguageCode]}", StringComparison.OrdinalIgnoreCase))
                return SaveStatus.Saved;
            else
                return SaveStatus.Unknown;

        }
    }
}
