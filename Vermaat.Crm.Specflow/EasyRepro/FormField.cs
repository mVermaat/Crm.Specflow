using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormField
    {

        private readonly string[] _controls;
        private readonly FormData _form;
        private readonly UCIApp _app;
        private readonly AttributeMetadata _metadata;

        private string _tabLabel;
        private string _tabName;

        public IEnumerable<string> Controls => _controls;

        public FormField(FormData form, UCIApp app, AttributeMetadata attributeMetadata, string[] controls)
        {
            _form = form;
            _app = app;
            _metadata = attributeMetadata;
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
                _tabLabel = _app.WebDriver.ExecuteScript($"return Xrm.Page.getControl('{GetDefaultControl()}').getParent().getParent().getLabel()")?.ToString();
            }
            return _tabLabel;
        }

        public RequiredState GetRequiredState()
        {
            BrowserCommandResult<RequiredState> result = _app.Client.Execute(BrowserOptionHelper.GetOptions($"Check field requirement"), driver =>
            {
                IWebElement fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", _metadata.LogicalName)));
                if (fieldContainer == null)
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, _metadata.LogicalName);

                if (fieldContainer.TryFindElement(By.XPath(Constants.XPath.FIELD_ISREQUIREDORRECOMMEND.Replace("[NAME]", _metadata.LogicalName)), out IWebElement requiredElement))
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

            return _app.WebDriver.WaitUntilVisible(By.XPath(
                AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", _metadata.LogicalName)), TimeSpan.FromSeconds(5));
        }

        public bool IsLocked()
        {
            return _app.Client.Execute(BrowserOptionHelper.GetOptions($"Check field locked state"), driver =>
            {
                IWebElement fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", _metadata.LogicalName)));
                if (fieldContainer == null)
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_ON_FORM, _metadata.LogicalName);

                return fieldContainer.TryFindElement(By.XPath(Constants.XPath.FIELD_ISLOCKED.Replace("[NAME]", _metadata.LogicalName)), out IWebElement requiredElement);
            }).Value;
        }

        public string GetTabName()
        {
            if (string.IsNullOrEmpty(_tabName))
            {
                _tabName = _app.WebDriver.ExecuteScript($"return Xrm.Page.getControl('{GetDefaultControl()}').getParent().getParent().getName()")?.ToString();
            }
            return _tabName;
        }

        public bool IsTabOfFieldExpanded()
        {

            string result = _app.WebDriver.ExecuteScript($"return Xrm.Page.ui.tabs.get('{GetTabName()}').getDisplayState()")?.ToString();
            return "expanded".Equals(result, StringComparison.CurrentCultureIgnoreCase);
        }

        public void SetValue(CrmTestingContext crmContext, string fieldValueText)
        {
            object fieldValue = ObjectConverter.ToCrmObject(_metadata.EntityLogicalName, _metadata.LogicalName, fieldValueText, crmContext, ConvertedObjectType.UserInterface);

            if (fieldValue != null)
            {
                Logger.WriteLine($"Setting field value");
                switch (_metadata.AttributeType.Value)
                {
                    case AttributeTypeCode.Boolean:
                        SetTwoOptionField((bool)fieldValue, fieldValueText);
                        break;
                    case AttributeTypeCode.DateTime:
                        SetDateTimeField((DateTime)fieldValue);
                        break;
                    case AttributeTypeCode.Customer:
                    case AttributeTypeCode.Lookup:
                        SetLookupValue((EntityReference)fieldValue);
                        break;
                    case AttributeTypeCode.Picklist:
                        SetOptionSetField((string)fieldValue);
                        break;
                    case AttributeTypeCode.Money:
                        SetMoneyField((Money)fieldValue);
                        break;
                    case AttributeTypeCode.Virtual:
                        SetVirtualField(fieldValue, fieldValueText);
                        break;
                    case AttributeTypeCode.Integer:
                    case AttributeTypeCode.Decimal:
                        SetTextField(fieldValueText);
                        break;
                    default:
                        SetTextField((string)fieldValue);
                        break;
                }
            }
            else
            {
                Logger.WriteLine($"Clearing field value");
                ClearValue(crmContext);
            }
        }

        private void SetVirtualField(object fieldValue, string fieldValueText)
        {
            if (_metadata.AttributeTypeName == AttributeTypeDisplayName.MultiSelectPicklistType)
                _app.App.Entity.SetValue(new MultiValueOptionSet { Name = _metadata.LogicalName, Values = fieldValueText.Split(',').Select(v => v.Trim()).ToArray() });
            else
                throw new NotImplementedException(string.Format("Virtual type {0} not implemented", _metadata.AttributeTypeName.Value));
        }

        private void ClearValue(CrmTestingContext crmContext)
        {
            switch (_metadata.AttributeType.Value)
            {
                case AttributeTypeCode.Boolean:
                    throw new TestExecutionException(Constants.ErrorCodes.TWO_OPTION_FIELDS_CANT_BE_CLEARED);
                case AttributeTypeCode.Customer:
                case AttributeTypeCode.Lookup:
                    _app.App.Entity.ClearValue(new LookupItem { Name = _metadata.LogicalName });
                    break;
                case AttributeTypeCode.Picklist:
                    _app.App.Entity.ClearValue(new OptionSet { Name = _metadata.LogicalName });
                    break;
                default:
                    SetTextField(null);
                    break;
            }
        }

        private void SetTwoOptionField(bool fieldValueBool, string fieldValueText)
        {
            _app.App.Entity.SetValue(new BooleanItem { Name = _metadata.LogicalName, Value = fieldValueBool });
        }

        private void SetDateTimeField(DateTime fieldValue)
        {
            string format = ((DateTimeAttributeMetadata)_metadata).Format == DateTimeFormat.DateAndTime
                ? GlobalTestingContext.ConnectionManager.CurrentUserDetails.UserSettings.DateTimeFormat
                : GlobalTestingContext.ConnectionManager.CurrentUserDetails.UserSettings.DateFormat;

            if (((DateTimeAttributeMetadata)_metadata).DateTimeBehavior == DateTimeBehavior.UserLocal)
            {
                var offset = GlobalTestingContext.ConnectionManager.CurrentUserDetails.UserSettings.TimeZoneInfo.GetUtcOffset(fieldValue);
                _app.App.Entity.SetValue(_metadata.LogicalName, fieldValue.Add(offset), format);
            }
            else
            {
                _app.App.Entity.SetValue(_metadata.LogicalName, fieldValue, format);
            }            
        }

        private void SetOptionSetField(string optionSetLabel)
        {
            _app.App.Entity.SetValue(new OptionSet { Name = _metadata.LogicalName, Value = optionSetLabel });
        }

        private void SetMoneyField(Money fieldValue)
        {
            SetTextField(fieldValue?.Value.ToString());
        }

        private void SetTextField(string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
                _app.App.Entity.ClearValue(_metadata.LogicalName);
            else
                SetValueFix(_metadata.LogicalName, fieldValue);
        }

        private void SetLookupValue(EntityReference fieldValue)
        {
            _app.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{_metadata.LogicalName}').setValue([ {{ id: '{fieldValue.Id}', name: '{fieldValue.Name}', entityType: '{fieldValue.LogicalName}' }} ])");
            _app.WebDriver.ExecuteScript($"Xrm.Page.getAttribute('{_metadata.LogicalName}').fireOnChange()");
        }

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.Entity.SetValue("firstname", "Test");</example>
        private BrowserCommandResult<bool> SetValueFix(string field, string value)
        {
            return _app.Client.Execute(BrowserOptionHelper.GetOptions($"Set Value"), driver =>
            {
                IWebElement fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));

                IWebElement input;
                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    input = fieldContainer.FindElement(By.TagName("input"));
                    
                }
                else if (fieldContainer.FindElements(By.TagName("textarea")).Count > 0)
                {
                    input = fieldContainer.FindElement(By.TagName("textarea"));
                }
                else
                {
                    throw new Exception($"Field with name {field} does not exist.");
                }

                if (input != null)
                {
                    string currentValue = input.GetAttribute("value");

                    input.Click();
                    if (!string.IsNullOrWhiteSpace(currentValue))
                    {
                        input.SendKeys(Keys.Control + "a");
                        input.SendKeys(Keys.Backspace);
                    }

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        input.SendKeys(value);
                    }

                    input.SendKeys(Keys.Tab + Keys.Tab);
                }

                return true;
            });
        }

    }
}
