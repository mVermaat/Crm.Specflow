using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class OpenRecordCommand : ISeleniumCommand
    {
        private readonly OpenFormOptions _formOptions;
        private readonly Guid? _currentAppId;

        public OpenRecordCommand(OpenFormOptions formOptions, Guid? currentAppId)
        {
            _formOptions = formOptions;
            _currentAppId = currentAppId;
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            Logger.WriteLine($"Opening record {_formOptions.EntityName} with ID {_formOptions.EntityId}");

            browserInteraction.Driver.Navigate().GoToUrl(_formOptions.GetUrl(browserInteraction.Driver, _currentAppId));

            CheckAlert(browserInteraction.Driver);

            if (HasMissingPermissionDialog(browserInteraction))
                return CommandResult.Fail(false, Constants.ErrorCodes.MISSING_PERMISSIONS_TO_VIEW_RECORD, _formOptions.EntityName, _formOptions.EntityId);

            HelperMethods.WaitForFormLoad(browserInteraction.Driver);
            CheckForWavePopup(browserInteraction.Driver);

            if (browserInteraction.Driver.HasElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_ScriptErrorDialog)))
                throw new TestExecutionException(Constants.ErrorCodes.FORMLOAD_SCRIPT_ERROR_ON_FORM);

            return CommandResult.Success();
        }

        private bool HasMissingPermissionDialog(BrowserInteraction browserInteraction)
        {
            browserInteraction.Driver.WaitForPageToLoad();
            browserInteraction.Driver.WaitForTransaction();
            if (!browserInteraction.Driver.TryFindElement(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_MissingPermisions_DialogRoot), out var dialogRoot))
                return false;

            var descriptionId = dialogRoot.GetAttribute("aria-describedby");
            if (string.IsNullOrEmpty(descriptionId))
                return false;

            var descriptionElement = dialogRoot.FindElement(By.Id(descriptionId));
            Logger.WriteLine($"Dialog description: {descriptionElement.GetAttribute("innerText")}");
            return true;
        }

        private void CheckForWavePopup(IWebDriver driver)
        {
            if (driver.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Popup_TeachingBubble_CloseButton), out var closeButton))
            {
                closeButton.Click();
            }
        }

        private void CheckAlert(IWebDriver driver)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch (NoAlertPresentException)
            {
            }
        }
    }
}
