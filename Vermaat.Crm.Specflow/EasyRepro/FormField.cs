using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Interactions;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormField : Field
    {

        private readonly string[] _controls;
        private readonly FormData _form;

        private string _tabLabel;
        private string _tabName;

        public IEnumerable<string> Controls => _controls;

        public FormField(FormData form, UCIApp app, AttributeMetadata attributeMetadata, string[] controls)
            : base(app, attributeMetadata)
        {
            _form = form;
            _controls = controls;
        }

        public string GetDefaultControl()
        {
            if (_controls.Length == 1)
                return _controls[0];
            else
                return _controls.FirstOrDefault(c => !c.StartsWith("header"));
        }

        public string GetTabLabel()
        {
            if (string.IsNullOrEmpty(_tabLabel))
            {
                _tabLabel = App.WebDriver.ExecuteScript($"return Xrm.Page.getControl('{GetDefaultControl()}').getParent().getParent().getLabel()")?.ToString();
            }
            return _tabLabel;
        }

        public RequiredState GetRequiredState()
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

        public bool IsVisible()
        {
            if (!IsTabOfFieldExpanded())
                _form.ExpandTab(GetTabLabel());

            return App.WebDriver.WaitUntilVisible(By.XPath(
                AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", LogicalName)), TimeSpan.FromSeconds(5));
        }

        public bool IsLocked()
        {
            return App.Client.Execute(BrowserOptionHelper.GetOptions($"Check field locked state"), driver =>
            {
                IWebElement fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", LogicalName)));
                if (fieldContainer == null)
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, LogicalName);

                return fieldContainer.TryFindElement(SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormState_LockedIcon, LogicalName), out IWebElement requiredElement);
            }).Value;
        }

        public string GetTabName()
        {
            if (string.IsNullOrEmpty(_tabName))
            {
                _tabName = App.WebDriver.ExecuteScript($"return Xrm.Page.getControl('{GetDefaultControl()}').getParent().getParent().getName()")?.ToString();
            }
            return _tabName;
        }

        public bool IsTabOfFieldExpanded()
        {

            string result = App.WebDriver.ExecuteScript($"return Xrm.Page.ui.tabs.get('{GetTabName()}').getDisplayState()")?.ToString();
            return "expanded".Equals(result, StringComparison.CurrentCultureIgnoreCase);
        }

        

        

    }
}
