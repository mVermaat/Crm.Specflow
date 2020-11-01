using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Controls
{
    internal abstract class FormField : Field
    {
        protected SeleniumExecutor Executor { get; private set; }

        public FormField(SeleniumExecutor executor, AttributeMetadata attributeMetadata, string control)
            : base(attributeMetadata, control)
        {
            Executor = executor;
        }

        public virtual RequiredState GetRequiredState(FormState formState)
        {
            return Executor.Execute("Get Required State", (driver, selectors) => 
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", LogicalName)));
                if (fieldContainer == null)
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, LogicalName);

                if (fieldContainer.TryFindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormState_RequiredOrRecommended, LogicalName), out IWebElement requiredElement))
                {
                    if (requiredElement.GetAttribute("innerText") == "*")
                        return RequiredState.Required;
                    else
                        return RequiredState.Recommended;
                }
                else
                    return RequiredState.Optional;
            });
        }

        public virtual bool IsVisible(FormState formState)
        {
            return Executor.Execute("Is Visible", (driver, selectors) =>
            {
                return driver.WaitUntilVisible(
                    selectors.GetXPathSeleniumSelector(
                        SeleniumSelectorItems.Entity_FieldContainer, 
                        ControlName),
                    TimeSpan.FromSeconds(5)) != null;
            });
        }

        public virtual bool IsLocked(FormState formState)
        {
            return Executor.Execute("Is Locked", (driver, selectors) =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", LogicalName)));
                if (fieldContainer == null)
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, LogicalName);

                return fieldContainer.TryFindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormState_LockedIcon, LogicalName), out IWebElement requiredElement);
            });
        }
    }
}
