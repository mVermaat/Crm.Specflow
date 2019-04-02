using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    abstract class BaseBrowserEntity : IBrowserEntity
    {
        protected readonly IBrowser _browser;

        public BaseBrowserEntity(IBrowser browser)
        {
            _browser = browser;
        }

        public void FillForm(CrmTestingContext crmContext, string entityName, Table dataTable)
        {
            //Entity entity = _browser.Entity;
            FormData formData = new FormData(crmContext, entityName, dataTable);
            var formFiller = CreateBrowserFiller();
            foreach (IFormField field in formData.Fields)
            {
                if (!IsFieldOnForm(field.FieldName))
                {
                    throw new InvalidOperationException($"Field {field.FieldName} is not on the form");
                }
                if (!IsTabOfFieldExpanded(field.FieldName))
                {
                    ExpandTabThatContainsField(field.FieldName);
                }
                field.EnterOnForm(_browser, formFiller);
            }
        }

        public string GetEntityName()
        {
            return Convert.ToString(GetWebDriver().ExecuteScript("return Xrm.Page.data.entity.getEntityName()"));
        }

        public bool IsFieldOnForm(string fieldLogicalName)
        {
            return Convert.ToBoolean(GetWebDriver().ExecuteScript($"return Xrm.Page.getControl('{fieldLogicalName}') != null"));
        }

        protected bool IsTabOfFieldExpanded(string fieldLogicalName)
        {
            string result = GetWebDriver().ExecuteScript($"return Xrm.Page.getControl('{fieldLogicalName}').getParent().getParent().getDisplayState()")?.ToString();
            return "expanded".Equals(result, StringComparison.CurrentCultureIgnoreCase);
        }

        protected void ExpandTabThatContainsField(string fieldLogicalName)
        {
            string tabName = GetWebDriver().ExecuteScript($"return Xrm.Page.getControl('{fieldLogicalName}').getParent().getParent().getLabel()")?.ToString();
            SelectTab(tabName);
        }


        public abstract void DeleteRecord();
        public abstract bool IsFieldVisible(string fieldLogicalName);
        public abstract void SaveRecord(bool saveIfDuplicate);
        public abstract Guid GetId();

        protected abstract IFormFiller CreateBrowserFiller();
        protected abstract IWebDriver GetWebDriver();
        protected abstract void SelectTab(string tabName);

    }
}
