using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Interactions;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    public abstract class FormField : Field
    {

        protected string Control { get; private set; }

        public FormField(UCIApp app, AttributeMetadata attributeMetadata, string control)
            : base(app, attributeMetadata)
        {
            Control = control;
        }

        public virtual RequiredState GetRequiredState(FormState formState)
        {
            BrowserCommandResult<RequiredState> result = App.Client.Execute(BrowserOptionHelper.GetOptions($"Check field requirement"), driver =>
            {
                IWebElement fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", LogicalName)));
                if (fieldContainer == null)
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, LogicalName);

                if (fieldContainer.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormState_RequiredOrRecommended, LogicalName), out IWebElement requiredElement))
                {
                    if (requiredElement.GetAttribute("innerText") == "*")
                        return RequiredState.Required;
                    else
                        return RequiredState.Recommended;
                }
                else
                {
                    return RequiredState.Optional;
                }
            });

            return result.Value;
        }

        public virtual bool IsVisible(FormState formState)
        {
            return App.WebDriver.WaitUntilVisible(
                SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FieldContainer, Control),
                TimeSpan.FromSeconds(5)) != null;
        }

        public virtual bool IsLocked(FormState formState)
        {
            return App.Client.Execute(BrowserOptionHelper.GetOptions($"Check field locked state"), driver =>
            {
                IWebElement fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", LogicalName)));
                if (fieldContainer == null)
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, LogicalName);

                return fieldContainer.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormState_LockedIcon, LogicalName), out IWebElement requiredElement);
            }).Value;
        }
    }
}
