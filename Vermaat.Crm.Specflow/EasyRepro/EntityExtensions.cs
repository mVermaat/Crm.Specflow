using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public static class EntityExtensions
    {
        public static BrowserCommandResult<bool> IsElementVisible(this Entity entity, string field)
        {
            return entity.Execute($"Is Field visible: {field}", driver =>
            {
                return driver.WaitUntilVisible(By.Id(field), TimeSpan.FromSeconds(5));
            });
        }

        /// <summary>
        /// Sets the value of a Text/Description field on an Entity form.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <param name="value">The value.</param>
        /// <example>xrmBrowser.Entity.SetValue("name", "Test API Account");</example>
        public static BrowserCommandResult<bool> SetValueFix(this Entity entity, string field, string value)
        {
            BrowserCommandResult<bool> returnval = entity.Execute(BrowserOptionHelper.GetOptions($"Set Text Field Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    driver.WaitUntilVisible(By.Id(field));

                    IWebElement fieldElement = driver.FindElement(By.Id(field));
                    if (fieldElement.IsVisible(By.TagName("a")))
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        IWebElement element = fieldElement.FindElement(By.TagName("a"));
                        js.ExecuteScript("arguments[0].setAttribute('style', 'pointer-events: none; cursor: default')", element);
                    }
                    fieldElement.Click();

                    try
                    {
                        //Check to see if focus is on field already
                        if (fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])) != null)
                            fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.EditClass])).Click();
                        else
                            fieldElement.FindElement(By.ClassName(Elements.CssClass[Reference.SetValue.ValueClass])).Click();
                    }
                    catch (NoSuchElementException) { }

                    if (fieldElement.FindElements(By.TagName("textarea")).Count > 0)
                    {
                        fieldElement.FindElement(By.TagName("textarea")).Clear();
                        fieldElement.FindElement(By.TagName("textarea")).SendKeys(value);
                    }
                    else if (fieldElement.TagName == "textarea")
                    {
                        fieldElement.Clear();
                        fieldElement.SendKeys(value);
                        fieldElement.SendKeys(Keys.Tab);
                    }
                    else
                    {
                        //BugFix - Setvalue -The value is getting erased even after setting the value ,might be due to recent CSS changes.
                        //driver.ExecuteScript("Xrm.Page.getAttribute('" + field + "').setValue('')");
                        IWebElement input = fieldElement.FindElement(By.TagName("input"));
                        if (HasValue(fieldElement))
                        {
                            input.Clear();
                            fieldElement.Click();
                            input.SendKeys(value + Keys.Tab);
                            fieldElement.Click();
                        }
                        else
                        {
                            input.SendKeys(value);
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
            return returnval;
        }

        /// <summary>
        /// Sets the value of a TwoOption / Checkbox field on an Entity form.
        /// </summary>
        /// <param name="option">Field name or ID.</param>
        /// <example>xrmBrowser.Entity.SetValue(new TwoOption{ Name = "creditonhold"});</example>
        public static BrowserCommandResult<bool> SetValueFix(this Entity entity, TwoOption option)
        {
            return entity.Execute(BrowserOptionHelper.GetOptions($"Set TwoOption Value: {option.Name}"), driver =>
            {
                if (driver.HasElement(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower()))))
                {
                    var fieldElement = driver.WaitUntilAvailable(By.XPath(Elements.Xpath[Reference.Entity.CheckboxFieldContainer].Replace("[NAME]", option.Name.ToLower())));

                    if (fieldElement.FindElements(By.TagName("label")).Count > 0)
                    {
                        var label = fieldElement.FindElement(By.TagName("label"));

                        if (label.Text != option.Value)
                        {
                            try
                            {
                                var input = fieldElement.FindElement(By.TagName("input"));
                                input.Click(true);
                            }
                            catch (NoSuchElementException)
                            {
                                fieldElement.Click(true);
                            }
                        }
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {option.Name} Does not exist");

                return true;
            });
        }

        private static bool HasValue(IWebElement fieldElement)
        {
            IWebElement label = fieldElement.FindElement(By.TagName("label"));
            var labelText = label.GetAttribute("innerText");

            IWebElement labelDiv = label.FindElement(By.TagName("div"));
            var labelDivText = labelDiv.GetAttribute("innerText");

            return labelText != labelDivText;
        }
    }
}
