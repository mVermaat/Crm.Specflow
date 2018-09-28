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
using Vermaat.Crm.Specflow.EasyRepro.Extensions;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    static class FormHelper
    {
        public static void FillForm(CrmTestingContext crmContext, Browser browser, string entityName, Table dataTable)
        {
            crmContext.TableConverter.ConvertTable(entityName, dataTable);

            var entity = browser.Entity;
            foreach (var row in dataTable.Rows)
            {
                var metadata = crmContext.Metadata.GetAttributeMetadata(entityName, row["Property"]);
                var crmObject = ObjectConverter.ToCrmObject(entityName, row["Property"], row["Value"], crmContext);

                if (!IsOnForm(browser.Driver, row["Property"]))
                {
                    throw new InvalidOperationException($"Field {row["Property"]} is not on the form");
                }
                if (!IsTabOfFieldExpanded(browser.Driver, row["Property"]))
                {
                    ExpandTabThatContainsField(browser, row["Property"]);
                }

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
                        entity.SetValueFix(row["Property"], row["Value"] + Keys.Tab, true);
                        entity.SetValueFix(row["Property"], Keys.Tab, false);
                        break;
                }
            }
        }

        internal static void SaveRecord(SeleniumTestingContext seleniumContext, bool saveIfDuplicate)
        {
            seleniumContext.Browser.Entity.Save();
            // ThinkTime before call to DuplicateDetection, frame must be visible before the method call in order to succeed
            seleniumContext.Browser.ThinkTime(2000);
            seleniumContext.Browser.Dialogs.DuplicateDetection(saveIfDuplicate);
            seleniumContext.Browser.ThinkTime(2000);
            seleniumContext.Browser.Entity.SwitchToContentFrame();
            
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

        public static bool IsOnForm(IWebDriver driver, string fieldLogicalName)
        {
            return Convert.ToBoolean(driver.ExecuteScript($"return Xrm.Page.getControl('{fieldLogicalName}') != null"));

        }

        public static bool IsTabOfFieldExpanded(IWebDriver driver, string fieldLogicalName)
        {
            var result = driver.ExecuteScript($"return Xrm.Page.getControl('{fieldLogicalName}').getParent().getParent().getDisplayState()")?.ToString();
            return "expanded".Equals(result, StringComparison.CurrentCultureIgnoreCase);
        }

        private static void ExpandTabThatContainsField(Browser browser, string fieldLogicalName)
        {
            var tabName = browser.Driver.ExecuteScript($"return Xrm.Page.getControl('{fieldLogicalName}').getParent().getParent().getLabel()")?.ToString();
            browser.Entity.SelectTab(tabName);
        }
    }
}
