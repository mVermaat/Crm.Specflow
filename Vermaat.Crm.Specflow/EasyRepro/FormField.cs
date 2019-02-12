using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    class FormField : IFormField
    {

        private readonly AttributeMetadata _fieldMetadata;

        public object FieldValue { get; }

        public string FieldName { get; }

        public FormField(CrmTestingContext crmContext, string entityName, string fieldName, string value)
        {
            FieldName = fieldName;

            _fieldMetadata = crmContext.Metadata.GetAttributeMetadata(entityName, fieldName);
            FieldValue = ObjectConverter.ToCrmObject(entityName, fieldName, value, crmContext, ConvertedObjectType.UserInterface);
           
        }

        public bool IsOnForm(IWebDriver driver)
        {
            return Convert.ToBoolean(driver.ExecuteScript($"return Xrm.Page.getControl('{FieldName}') != null"));
        }

        public void EnterOnForm(Entity entity)
        {
            switch (_fieldMetadata.AttributeType.Value)
            {
                case AttributeTypeCode.Boolean:
                    entity.SetValue(new TwoOption() { Name = FieldName, Value = (string)FieldValue });
                    break;
                case AttributeTypeCode.DateTime:
                    entity.SetValue(new DateTimeControl() { Name = FieldName, Value = (DateTime)FieldValue });
                    break;
                case AttributeTypeCode.Lookup:
                    entity.SetValue(new LookupItem { Name = FieldName, Value = (string)FieldValue });
                    break;
                case AttributeTypeCode.Picklist:
                    entity.SetValue(new OptionSet { Name = FieldName, Value = (string)FieldValue });
                    break;
                default:
                    entity.SetValue(FieldName,(string)FieldValue);
                    break;
            }
        }
    }
}
