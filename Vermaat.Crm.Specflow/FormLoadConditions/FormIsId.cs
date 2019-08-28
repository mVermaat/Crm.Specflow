using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.FormLoadConditions
{
    public class FormIsId : IFormLoadCondition
    {
        private readonly Guid _recordId;
        private readonly bool _invert;

        public FormIsId(Guid recordId, bool invert)
        {
            _recordId = recordId;
            _invert = invert;
        }

        public bool Evaluate(IWebDriver driver)
        {
            try
            {
                Logger.WriteLine($"Evaluating form load - Is current record ID {_recordId}");
                var actualId = driver.ExecuteScript("return Xrm.Page.data.entity.getId();") as string;

                return  Guid.TryParse(actualId, out Guid parsedId) && 
                        _recordId == parsedId != _invert;
            }
            catch (WebDriverException)
            {
                return false;
            }
        }
    }
    
}
