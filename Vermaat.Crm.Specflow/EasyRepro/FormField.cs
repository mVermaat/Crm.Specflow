using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
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

        public void EnterOnForm(IBrowser browser, IFormFiller formFiller)
        {
            switch (_fieldMetadata.AttributeType.Value)
            {
                case AttributeTypeCode.Boolean:
                    formFiller.SetTwoOptionField(FieldName, (bool)FieldValue);
                    break;
                case AttributeTypeCode.DateTime:
                    formFiller.SetDateTimeField(FieldName, (DateTime)FieldValue);
                    break;
                case AttributeTypeCode.Customer:
                case AttributeTypeCode.Lookup:
                    formFiller.SetLookupValue(FieldName, (EntityReference)FieldValue);
                    break;
                case AttributeTypeCode.Picklist:
                    formFiller.SetOptionSetField(FieldName, (string)FieldValue);
                    break;
                default:
                    formFiller.SetTextField(FieldName, (string)FieldValue);
                    break;
            }
        }
    }
}
