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

        public void EnterOnForm(Browser browser, Microsoft.Dynamics365.UIAutomation.Api.Entity entity)
        {
            switch (_fieldMetadata.AttributeType.Value)
            {
                case AttributeTypeCode.Boolean:
                    entity.SetValueFix(new TwoOption() { Name = FieldName, Value = (string)FieldValue });
                    break;
                case AttributeTypeCode.DateTime:
                    entity.SetValue(new DateTimeControl() { Name = FieldName, Value = (DateTime)FieldValue });
                    break;
                case AttributeTypeCode.Customer:
                case AttributeTypeCode.Lookup:
                    SetLookupValue(browser, entity, (EntityReference)FieldValue);
                    //entity.SetValue(new LookupEntityReference { FieldName = FieldName, Value = (EntityReference)FieldValue });
                    break;
                case AttributeTypeCode.Picklist:
                    entity.SetValue(new OptionSet { Name = FieldName, Value = (string)FieldValue });
                    break;
                default:
                    entity.SetValueFix(FieldName,(string)FieldValue);
                    break;
            }
        }

        private void SetLookupValue(Browser browser, Microsoft.Dynamics365.UIAutomation.Api.Entity entity, EntityReference value)
        {
            entity.SelectLookup(new LookupItem { Name = FieldName });

            var lookup = browser.Lookup;
            if (!string.IsNullOrEmpty(value.Name))
                lookup.Search(value.Name);

            var index = FindGridItemIndex(value, lookup);

            if (index == null)
                throw new ArgumentException($"Lookup not found. Was looking for Entity: {value.Id} ({value.Name}) of type {value.LogicalName}");

            lookup.SelectItem(index.Value);
            lookup.Add();
            entity.SwitchToContentFrame();
        }

        private int? FindGridItemIndex(EntityReference value, Lookup lookup)
        {
            var gridItems = lookup.GetGridItems();

            for(int i = 0; i < gridItems.Value.Count; i++)
            {
                if (gridItems.Value[i].Id == value.Id)
                    return i;
            }

            var next = lookup.NextPage();

            if (next.Success.GetValueOrDefault())
                return FindGridItemIndex(value, lookup);
            else
                return null;

        }
    }
}
