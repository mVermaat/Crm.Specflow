using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    static class FormHelper
    {
        public static void FillForm(CrmTestingContext crmContext, Browser browser, string entityName, Table dataTable)
        {
            var entity = browser.Entity;
            var formData = new FormData(crmContext, entityName, dataTable);
            foreach (var field in formData.Fields)
            {
                if(!field.IsOnForm(browser.Driver))
                {
                    throw new InvalidOperationException($"Field {field.FieldName} is not on the form");
                }
                if (!IsTabOfFieldExpanded(browser.Driver, field.FieldName))
                {
                    ExpandTabThatContainsField(browser, field.FieldName);
                }
                field.EnterOnForm(entity);
            }
        }

        internal static void SaveRecord(SeleniumTestingContext seleniumContext, bool saveIfDuplicate)
        {
            seleniumContext.Browser.CommandBar.ClickCommand("SAVE");
            // ThinkTime before call to DuplicateDetection, frame must be visible before the method call in order to succeed
            seleniumContext.Browser.ThinkTime(2000);
            seleniumContext.Browser.Dialogs.DuplicateDetection(saveIfDuplicate);
            seleniumContext.Browser.ThinkTime(2000);
            seleniumContext.Browser.Entity.SwitchToContentFrame();

        }

        public static string GetSubgridName(string entityName, IWebDriver driver)
        {
            object result = driver.ExecuteScript($"return Xrm.Page.ui.controls.getAll().filter(t => {{ return t.getControlType() === 'subgrid' && t.getEntityName() === '{entityName}' }})[0].getName()");

            return result.ToString();
        }

        public static FormType GetFormType(IWebDriver driver)
        {
            return (FormType)Convert.ToInt32(driver.ExecuteScript($"return Xrm.Page.ui.getFormType()"));
        }

        public static EntityReference AddAlias(CrmTestingContext crmContext, IWebDriver driver, string alias)
        {
            EntityReference entity = GetEntityReference(driver);
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

        public static bool IsTabOfFieldExpanded(IWebDriver driver, string fieldLogicalName)
        {
            string result = driver.ExecuteScript($"return Xrm.Page.getControl('{fieldLogicalName}').getParent().getParent().getDisplayState()")?.ToString();
            return "expanded".Equals(result, StringComparison.CurrentCultureIgnoreCase);
        }

        private static void ExpandTabThatContainsField(Browser browser, string fieldLogicalName)
        {
            string tabName = browser.Driver.ExecuteScript($"return Xrm.Page.getControl('{fieldLogicalName}').getParent().getParent().getLabel()")?.ToString();
            browser.Entity.SelectTab(tabName);
        }
    }
}
