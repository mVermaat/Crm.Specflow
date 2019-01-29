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
        /// Sets the value of a Text/Description field.
        /// </summary>
        /// <param name="field">The field id.</param>
        /// <param name="value">The value.</param>
        /// <example>xrmBrowser.Entity.SetValue("name", "Test API Account");</example>
        public static BrowserCommandResult<bool> SetValueFix(this Entity entity, string field, string value, bool clear)
        {
            //return this.Execute($"Set Value: {field}", SetValue, field, value);
            var returnval = entity.Execute(BrowserOptionHelper.GetOptions($"Set Value: {field}"), driver =>
            {
                if (driver.HasElement(By.Id(field)))
                {
                    driver.WaitUntilVisible(By.Id(field));

                    var fieldElement = driver.FindElement(By.Id(field));
                    if (fieldElement.IsVisible(By.TagName("a")))
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        var element = fieldElement.FindElement(By.TagName("a"));
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
                    }
                    else
                    {
                        //BugFix - Setvalue -The value is getting erased even after setting the value ,might be due to recent CSS changes.
                        //driver.ExecuteScript("Xrm.Page.getAttribute('" + field + "').setValue('')");
                        var element = fieldElement.FindElement(By.TagName("input"));
                        element.SendKeys(value, clear);
                    }
                }
                else
                    throw new InvalidOperationException($"Field: {field} Does not exist");

                return true;
            });
            return returnval;
        }

        
    }
}
