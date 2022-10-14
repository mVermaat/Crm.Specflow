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
            var status = GetSaveStatus(browserInteraction);

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

            SeleniumCommandProcessor.ExecuteCommand(browserInteraction, new ClickRibbonItemCommand(browserInteraction.LocalizedTexts[Constants.LocalizedTexts.SaveButton, browserInteraction.UiLanguageCode]));

            if (HasDuplicateDetection(browserInteraction))
            {
                if (_saveIfDuplicate)
                    AcceptDuplicateDetection(browserInteraction.Driver);
                else
                    return CommandResult.Fail(false, Constants.ErrorCodes.FORM_SAVE_FAILED, $"Duplicate detected and saving with duplicate is not allowed");
            }
            if (browserInteraction.Driver.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_ErrorDialog), out var errorDialog))
            {
                var errorDetails = errorDialog.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Subtitle));
                if (!string.IsNullOrEmpty(errorDetails.Text))
                    return CommandResult.Fail(false, Constants.ErrorCodes.FORM_SAVE_FAILED, errorDetails.Text);
            }

            var timeout = DateTime.Now.AddSeconds(20);
            var unsavedTimeout = DateTime.Now.AddSeconds(5);
            bool saveCompleted = false;
            while (!saveCompleted && DateTime.Now < timeout)
            {
                var saveStatus = GetSaveStatus(browserInteraction);

                if (saveStatus == SaveStatus.Saved)
                    saveCompleted = true;
                else if (saveStatus == SaveStatus.Unsaved && DateTime.Now > unsavedTimeout)
                {
                    var formNotifications = SeleniumCommandProcessor.ExecuteCommand(browserInteraction, new GetFormNotificationsCommand());
                    return CommandResult.Fail(false, Constants.ErrorCodes.FORM_SAVE_FAILED, $"Detected Unsaved changes. Form Notifications: {string.Join(", ", formNotifications)}");

                }
                else
                    Thread.Sleep(200);
            }

            if (!saveCompleted)
                return CommandResult.Fail(false, Constants.ErrorCodes.FORM_SAVE_TIMEOUT, 20);

            Logger.WriteLine("Save sucessfull");
            return CommandResult.Success();
        }

        private bool HasDuplicateDetection(BrowserInteraction browserInteraction)
        {
            var timeout = DateTime.Now.AddSeconds(5);
            bool isSaving = false;

            while (DateTime.Now < timeout)
            {
                if (browserInteraction.Driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionWindowMarker])) &&
                    browserInteraction.Driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows])))
                    return true;

                if (browserInteraction.Driver.HasElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_ErrorDialog)))
                    return false;

                var status = GetSaveStatus(browserInteraction);
                if (status == SaveStatus.Saved || status == SaveStatus.Saving)
                {
                    // When duplicate detection is found it will go to saving and then back to unsaved.
                    // This goes really fast, so this will make sure it was saving/saved and still saving/saved after 100ms
                    if (isSaving)
                        return false;
                    else
                        isSaving = true;
                }

                Thread.Sleep(100);
            }
            return false;


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
            var saveStatusText = browserInteraction.Driver.FindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_SaveStatus))?.Text;

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
