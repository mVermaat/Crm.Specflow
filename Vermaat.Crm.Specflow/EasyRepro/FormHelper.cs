﻿using Microsoft.Dynamics365.UIAutomation.Api;
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
                var metadata = crmContext.Metadata.GetAttributeMetadata(entityName, row[Constants.SpecFlow.TABLE_KEY]);
                var crmObject = ObjectConverter.ToCrmObject(entityName, row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE], crmContext);

                if (!IsOnForm(browser.Driver, row[Constants.SpecFlow.TABLE_KEY]))
                {
                    throw new InvalidOperationException($"Field {row[Constants.SpecFlow.TABLE_KEY]} is not on the form");
                }
                if (!IsTabOfFieldExpanded(browser.Driver, row[Constants.SpecFlow.TABLE_KEY]))
                {
                    ExpandTabThatContainsField(browser, row[Constants.SpecFlow.TABLE_KEY]);
                }

                switch (metadata.AttributeType.Value)
                {

                    case AttributeTypeCode.Boolean:
                        entity.SetValue(row[Constants.SpecFlow.TABLE_KEY], (bool)crmObject);
                        break;
                    case AttributeTypeCode.DateTime:
                        entity.SetValue(row[Constants.SpecFlow.TABLE_KEY], (DateTime)crmObject);
                        break;
                    case AttributeTypeCode.Lookup:
                        entity.SetValue(new LookupItem { Name = row[Constants.SpecFlow.TABLE_KEY], Value = row[Constants.SpecFlow.TABLE_VALUE] });
                        break;
                    case AttributeTypeCode.Picklist:
                        entity.SetValue(new OptionSet { Name = row[Constants.SpecFlow.TABLE_KEY], Value = row[Constants.SpecFlow.TABLE_VALUE] });
                        break;
                    default:
                        entity.SetValueFix(row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE] + Keys.Tab, true);
                        entity.SetValueFix(row[Constants.SpecFlow.TABLE_KEY], Keys.Tab, false);
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

        public static EntityReference AddAlias(CrmTestingContext crmContext, IWebDriver driver, string alias)
        {
            var entity = GetEntityReference(driver);
            crmContext.RecordCache.Add(alias, entity);
            return entity;
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
