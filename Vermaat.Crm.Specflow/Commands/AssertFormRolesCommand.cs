using Microsoft.Xrm.Sdk;
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
        private readonly string[] _roles;

        public AssertFormRolesCommand(CrmTestingContext crmContext, string entityTypeCode, string name, string[] roles) 
            : base(crmContext)
        {
            _entityTypeCode = entityTypeCode;
            _name = name;
            _roles = roles;
        }

        public override void Execute()
        {
            var form = SystemForm.GetSystemForm(GlobalTestingContext.ConnectionManager.AdminConnection, _name, _entityTypeCode);
            if(form == null)
            {
                if (!string.IsNullOrEmpty(_entityTypeCode) && _entityTypeCode.Equals("none", StringComparison.OrdinalIgnoreCase))
                    throw new TestExecutionException(Constants.ErrorCodes.DASHBOARD_NOT_FOUND, _name);
                else
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_NOT_FOUND, _name, _entityTypeCode);
            }

            var xml = form.FormXml;
                
        }

    }
}
