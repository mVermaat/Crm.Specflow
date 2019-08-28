using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;

namespace Vermaat.Crm.Specflow.FormLoadConditions
{
    public class FormIsOfEntity : IFormLoadCondition
    {
        private readonly string _entityName;

        public FormIsOfEntity(string entityName)
        {
            _entityName = entityName;
        }

        public bool Evaluate(IWebDriver driver)
        {
            try
            {
                Logger.WriteLine($"Evaluating form load - Is current loaded form of type {_entityName}");
                var actualEntityName = driver.ExecuteScript("return Xrm.Page.data.entity.getEntityName();") as string;
                return !string.IsNullOrEmpty(actualEntityName) && actualEntityName.Equals(_entityName, StringComparison.CurrentCultureIgnoreCase);
            }
            catch(WebDriverException)
            {
                return false;
            }
        }
    }
}
