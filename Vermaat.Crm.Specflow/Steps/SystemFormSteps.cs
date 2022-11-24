using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow.Steps
{
    [Binding]
    public class SystemFormSteps
    {
        private readonly CrmTestingContext _crmContext;

        public SystemFormSteps(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
        }

        [Then(@"dashboard '(.*)' is accessible by following roles: (.*)")]
        public void ThenDashboardIsAvailableForRoles(string dashboardName, string roles)
        {
            _crmContext.CommandProcessor.Execute(new AssertFormRolesCommand(_crmContext, "none", dashboardName, ParseRoles(roles)));
        }

        [Then(@"dashboard '(.*)' is accessible by following roles")]
        public void ThenDashboardIsAvailableForRoles(string dashboardName, Table roles)
        {
            _crmContext.CommandProcessor.Execute(new AssertFormRolesCommand(_crmContext, "none", dashboardName, ParseRoles(roles)));
        }

        [Then(@"form '(.*)' of ([^\s]+) is accessible by following roles: (.*)")]
        public void ThenFormIsAvailableForRoles(string formName, string entityName, string roles)
        {
            _crmContext.CommandProcessor.Execute(new AssertFormRolesCommand(_crmContext, entityName, formName, ParseRoles(roles)));

        }

        [Then(@"form '(.*)' of ([^\s]+) is accessible by following roles")]
        public void ThenFormIsAvailableForRoles(string formName, string entityName, Table roles)
        {
            _crmContext.CommandProcessor.Execute(new AssertFormRolesCommand(_crmContext, entityName, formName, ParseRoles(roles)));
        }

        private string[] ParseRoles(Table roles)
        {
            return roles.Rows.Select(r => r[Constants.SpecFlow.TABLE_ROLE]?.Trim()).ToArray();
        }

        private string[] ParseRoles(string roles)
        {
           if(string.IsNullOrWhiteSpace(roles))
                return Array.Empty<string>();

            return roles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s?.Trim()).ToArray();
        }
    }
}
