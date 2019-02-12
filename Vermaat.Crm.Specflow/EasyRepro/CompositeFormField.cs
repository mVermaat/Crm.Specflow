using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    class CompositeFormField : IFormField
    {
        private List<FormField> _fields;

        public string FieldName { get; }

        public CompositeFormField(string parentField)
        {
            _fields = new List<FormField>();
            FieldName = parentField;
        }

        public void AddField(FormField field)
        {
            _fields.Add(field);
        }

        public bool IsOnForm(IWebDriver driver)
        {
            return Convert.ToBoolean(driver.ExecuteScript($"return Xrm.Page.getControl('{FieldName}') != null"));
        }

        public void EnterOnForm(Entity entity)
        {
            var composite = new CompositeControl
            {
                Id = FieldName,
                Fields = _fields.Select(f => new Field { Id = f.FieldName, Value = (string)f.FieldValue }).ToList()
            };
            entity.SetValue(composite, true);
            entity.SwitchToContentFrame();
        }
    }
}
