using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Commands
{
    public class AssertFormRolesCommand : ApiOnlyCommand
    {
        private readonly string _entityTypeCode;
        private readonly string _name;
        private readonly string[] _expectedRoles;

        public AssertFormRolesCommand(CrmTestingContext crmContext, string entityTypeCode, string name, IEnumerable<string> roles) 
            : base(crmContext)
        {
            _entityTypeCode = entityTypeCode;
            _name = name;
            _expectedRoles = roles.Select(r => r.ToLowerInvariant()).ToArray();
        }

        public override void Execute()
        {
            var form = SystemForm.GetSystemForm(GlobalTestingContext.ConnectionManager.CurrentConnection, _name, _entityTypeCode);
            if(form == null)
            {
                if (!string.IsNullOrEmpty(_entityTypeCode) && _entityTypeCode.Equals("none", StringComparison.OrdinalIgnoreCase))
                    throw new TestExecutionException(Constants.ErrorCodes.DASHBOARD_NOT_FOUND, _name);
                else
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_NOT_FOUND, _name, _entityTypeCode);
            }
            var actualRoles = GetRoles(form.FormXml.DisplayConditions?.Select(r => Guid.Parse(r.Id))?.ToArray());

            var extraRoles = actualRoles.Except(_expectedRoles).ToArray();
            var missingRoles = _expectedRoles.Except(actualRoles).ToArray();

            if(extraRoles.Length > 0 || missingRoles.Length > 0)
            {
                throw new TestExecutionException(Constants.ErrorCodes.ROLE_COUNT_ASSERT_FAILED, _name, 
                    missingRoles.Length, string.Join(", ", missingRoles),
                    extraRoles.Length, string.Join(", ", extraRoles));
            }
        }

        private string[] GetRoles(Guid[] roleIds)
        {
            if(roleIds == null || roleIds.Length == 0)
                return Array.Empty<string>();

            return GlobalTestingContext.ConnectionManager.AdminConnection.RetrieveMultiple(new QueryExpression("role")
            {
                ColumnSet = new ColumnSet("name"),
                NoLock = true,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("roleid", ConditionOperator.In, roleIds)
                    }
                }
            })
            .Entities
            .Select(e => e.GetAttributeValue<string>("name")?.ToLowerInvariant())
            .ToArray();
        }
    }
}
