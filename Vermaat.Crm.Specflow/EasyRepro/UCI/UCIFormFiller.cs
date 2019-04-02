using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vermaat.Crm.Specflow.EasyRepro.UCI
{
    class UCIFormFiller : IFormFiller
    {
        private readonly Microsoft.Dynamics365.UIAutomation.Api.UCI.Entity _entity;
        private readonly WebClient _client;

        public UCIFormFiller(Microsoft.Dynamics365.UIAutomation.Api.UCI.Entity entity, WebClient client)
        {
            _entity = entity;
            _client = client;
        }

        public void SetCompositeField(string parentField, IEnumerable<(string fieldName, string fieldValue)> fields)
        {
            foreach (var (fieldName, fieldValue) in fields)
            {
                SetTextField(fieldName, fieldValue);
            }
        }

        public void SetDateTimeField(string fieldName, DateTime fieldValue)
        {
            _entity.SetValue(fieldName, fieldValue);
        }

        public void SetLookupValue(string fieldName, EntityReference value)
        {
            throw new NotImplementedException();
        }

        public void SetOptionSetField(string fieldName, string fieldValue)
        {
            _entity.SetValue(new OptionSet { Name = fieldName, Value = fieldValue });
        }

        public void SetTextField(string fieldName, string fieldValue)
        {
            _entity.ClearValue(fieldName);
            SetValue(fieldName, fieldValue);
        }

        public void SetTwoOptionField(string fieldName, bool fieldValue)
        {
            _entity.SetValue(new BooleanItem { Name = fieldName, Value = fieldValue });
        }

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="value">The value</param>
        /// <example>xrmApp.Entity.SetValue("firstname", "Test");</example>
        private BrowserCommandResult<bool> SetValue(string field, string value)
        {
            return _client.Execute(BrowserOptionHelper.GetOptions($"Set Value"), driver =>
            {
                var fieldContainer = driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", field)));

                if (fieldContainer.FindElements(By.TagName("input")).Count > 0)
                {
                    var input = fieldContainer.FindElement(By.TagName("input"));
                    if (input != null)
                    {
                        var currentValue = input.GetAttribute("value");
                        var stringBuilder = new StringBuilder(value.Length + currentValue.Length);
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            for (int i = 0; i < currentValue.Length; i++)
                            {
                                stringBuilder.Append(Keys.Backspace);
                            }
                        }
                        stringBuilder.Append(value);

                        input.Click();
                        input.SendKeys(stringBuilder.ToString());
                    }
                }
                else if (fieldContainer.FindElements(By.TagName("textarea")).Count > 0)
                {
                    fieldContainer.FindElement(By.TagName("textarea")).Click();
                    fieldContainer.FindElement(By.TagName("textarea")).Clear();
                    fieldContainer.FindElement(By.TagName("textarea")).SendKeys(value);
                }
                else
                {
                    throw new Exception($"Field with name {field} does not exist.");
                }

                return true;
            });
        }
    }
}
