using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;
using Vermaat.Crm.Specflow.Entities;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro.Commands;
using Vermaat.Crm.Specflow.Connectivity;

namespace Vermaat.Crm.Specflow.Commands
{
    internal class AssertUserAccessCommand : BrowserOnlyCommand
    {
        private readonly UserProfileHandler _userProfileHandler;
        private readonly string _alias;
        private readonly Table _userAccessData;

        public AssertUserAccessCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, UserProfileHandler userProfileHandler, string alias, Table accessCriteria)
            : base(crmContext, seleniumContext)
        {
            _userProfileHandler = userProfileHandler;
            _alias = alias;
            _userAccessData = accessCriteria;
        }

        public override void Execute()
        {
            var record = _crmContext.RecordCache.Get(_alias, true);
            var browser = _seleniumContext.GetBrowser();
            var formData = browser.OpenRecord(new OpenFormOptions(record));

            formData.CommandBar.ClickButton(browser.App.LocalizedTexts["CheckAccessRibbonButton", browser.App.UILanguageCode]);

            var errors = new List<string>();
            foreach (var row in _userAccessData.Rows)
            {
                var profile = _userProfileHandler.GetProfile(row[Constants.SpecFlow.TABLE_FORMSTATE]);
                var expectedAccess = ParseTableRow(row[Constants.SpecFlow.TABLE_PERMISSIONS]);
                var userConnection = CrmConnectionFactory.CreateNewConnection(profile);

                // get correct session
                var userBrowser = GlobalTestingContext.BrowserManager.GetBrowser(_seleniumContext.BrowserOptions, 
                    userConnection.GetBrowserLoginInformation(), _seleniumContext.SeleniumCommandFactory);
                var access = SeleniumCommandProcessor.ExecuteCommand(userBrowser.App, userBrowser.App.SeleniumCommandFactory.CreateGetAccessForUserCommand());

                errors.AddRange(AssertAccess(expectedAccess, access));
            }
        }

        private IEnumerable<string> AssertAccess(UserAccessData actualAccessData, UserAccessData expectedAccessData)
        {
            if (actualAccessData.HasAppendAccess != expectedAccessData.HasAppendAccess)
                yield return $"Expected append access {expectedAccessData.HasAppendAccess} | Actual: {actualAccessData.HasAppendAccess}";

            if (actualAccessData.HasAssignAccess != expectedAccessData.HasAssignAccess)
                yield return $"Expected assign access {expectedAccessData.HasAssignAccess} | Actual: {actualAccessData.HasAssignAccess}";

            if (actualAccessData.HasAppendToAccess != expectedAccessData.HasAppendToAccess)
                yield return $"Expected append to access {expectedAccessData.HasAppendToAccess} | Actual: {actualAccessData.HasAppendToAccess}";

            if (actualAccessData.HasDeleteAccess != expectedAccessData.HasDeleteAccess)
                yield return $"Expected delete access {expectedAccessData.HasDeleteAccess} | Actual: {actualAccessData.HasDeleteAccess}";

            if (actualAccessData.HasCreateAccess != expectedAccessData.HasCreateAccess)
                yield return $"Expected create access {expectedAccessData.HasCreateAccess} | Actual: {actualAccessData.HasCreateAccess}";

            if (actualAccessData.HasReadAccess != expectedAccessData.HasReadAccess)
                yield return $"Expected read access {expectedAccessData.HasReadAccess} | Actual: {actualAccessData.HasReadAccess}";

            if (actualAccessData.HasShareAccess != expectedAccessData.HasShareAccess)
                yield return $"Expected share access {expectedAccessData.HasShareAccess} | Actual: {actualAccessData.HasShareAccess}";

            if (actualAccessData.HasWriteAccess != expectedAccessData.HasWriteAccess)
                yield return $"Expected write access {expectedAccessData.HasWriteAccess} | Actual: {actualAccessData.HasWriteAccess}";
        }

        private UserAccessData ParseTableRow(string rowData)
        {
            var permissions = rowData.Split(',').Select(s => s.Trim().ToLower());
            var result = new UserAccessData();

            foreach(var permissionString in permissions)
            {
                switch(permissionString)
                {
                    case "read": result.HasReadAccess = true; break;
                    case "create": result.HasCreateAccess = true; break;
                    case "write": result.HasWriteAccess = true; break;
                    case "delete": result.HasDeleteAccess = true; break;
                    case "append": result.HasAppendAccess = true; break;
                    case "appendto": result.HasAppendToAccess = true; break;    
                    case "share": result.HasShareAccess = true; break;
                    case "assign": result.HasAssignAccess = true; break;
                    default: throw new TestExecutionException(Constants.ErrorCodes.CHECK_ACCESS_WRONG_ACCESS_EXPECTATION_TEXT, permissionString, "read, create, write, delete, append, append to, share, assign");
                }
            }

            return result;
        }
    }
}
