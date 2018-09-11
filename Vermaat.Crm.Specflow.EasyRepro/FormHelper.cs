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
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    static class FormHelper
    {
        public static void FillForm(CrmTestingContext crmContext, Microsoft.Dynamics365.UIAutomation.Api.Entity entity, string entityName, Table dataTable)
        {
            crmContext.TableConverter.ConvertTable(entityName, dataTable);

            foreach (var row in dataTable.Rows)
            {
                var metadata = crmContext.Metadata.GetAttributeMetadata(entityName, row["Property"]);
                var crmObject = ObjectConverter.ToCrmObject(entityName, row["Property"], row["Value"], crmContext);

                switch (metadata.AttributeType.Value)
                {

                    case AttributeTypeCode.Boolean:
                        entity.SetValue(row["Property"], (bool)crmObject);
                        break;
                    case AttributeTypeCode.DateTime:
                        entity.SetValue(row["Property"], (DateTime)crmObject);
                        break;
                    case AttributeTypeCode.Lookup:
                        entity.SetValue(new LookupItem { Name = row["Property"], Value = row["Value"] });
                        break;
                    case AttributeTypeCode.Picklist:
                        entity.SetValue(new OptionSet { Name = row["Property"], Value = row["Value"] });
                        break;
                    default:
                        entity.SetValue(row["Property"], row["Value"]);
                        break;
                }
            }
        }

        public static string GetSubgridName(string entityName, IWebDriver driver)
        {
            var result = driver.ExecuteScript($"return Xrm.Page.ui.controls.getAll().filter(t => {{ return t.getControlType() === 'subgrid' && t.getEntityName() === '{entityName}' }})[0].getName()");

            return result.ToString();
        }

        public static FormType GetFormType(IWebDriver driver)
        {
            return (FormType)Convert.ToInt32(driver.ExecuteScript($"return Xrm.Page.ui.getFormType()"));
        }

        public static void AddAlias(CrmTestingContext crmContext, IWebDriver driver, string alias)
        {
            crmContext.RecordCache.Add(alias, GetEntityReference(driver));
        }

        public static EntityReference GetEntityReference(IWebDriver driver)
        {
            return new EntityReference(GetEntityName(driver), GetId(driver));
        }

        public static Guid GetId(IWebDriver driver)
        {
            return Guid.Parse(driver.ExecuteScript("return Xrm.Page.data.entity.getId()").ToString());
        }

        public static string GetEntityName(IWebDriver driver)
        {
            return Convert.ToString(driver.ExecuteScript("return Xrm.Page.data.entity.getEntityName()"));
        }
    }
}
