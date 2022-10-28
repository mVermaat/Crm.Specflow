using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Connectivity
{
    public class HybridCrmConnection : CrmConnection
    {
        private readonly BrowserLoginDetails _loginInfo;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public HybridCrmConnection(string clientId, string clientSecret, string browserUsername, string browserPassword, SecureString browserMfaKey = null)
            : base(browserUsername)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _loginInfo = new BrowserLoginDetails
            {
                Url = HelperMethods.GetAppSettingsValue("Url", false),
                Username = browserUsername,
                Password = browserPassword.ToSecureString(),
                MfaKey = browserMfaKey,
            };
        }

        public static HybridCrmConnection CreateFromAppConfig()
        {
            return new HybridCrmConnection(
                HelperMethods.GetAppSettingsValue("ClientId", false),
                HelperMethods.GetAppSettingsValue("ClientSecret", false),
                HelperMethods.GetAppSettingsValue("Username", false),
                HelperMethods.GetAppSettingsValue("Password", false),
                HelperMethods.GetAppSettingsValue("MfaKey", true)?.ToSecureString());
        }

        public static HybridCrmConnection CreateAdminConnectionFromAppConfig()
        {
            var clientId = HelperMethods.GetAppSettingsValue("AdminClientId", true) ?? HelperMethods.GetAppSettingsValue("ClientId");
            var clientSecret = HelperMethods.GetAppSettingsValue("AdminClientSecret", true) ?? HelperMethods.GetAppSettingsValue("ClientSecret");
            var username = HelperMethods.GetAppSettingsValue("AdminUsername", true) ?? HelperMethods.GetAppSettingsValue("Username");
            var password = HelperMethods.GetAppSettingsValue("AdminPassword", true) ?? HelperMethods.GetAppSettingsValue("Password");
            var mfaKey = HelperMethods.GetAppSettingsValue("AdminMfaKey", true)?.ToSecureString() ?? HelperMethods.GetAppSettingsValue("MfaKey", true)?.ToSecureString();
            return new HybridCrmConnection(clientId, clientSecret, username, password, mfaKey);
        }

        public override CrmService CreateCrmServiceInstance()
        {
            var service = new CrmService($"AuthType=ClientSecret;Url='{_loginInfo.Url}';ClientId='{_clientId}';ClientSecret='{_clientSecret}';RequireNewInstance=True");
            service.CallerId = GetImpersonatingUser(service);
            return service;
        }

        public override BrowserLoginDetails GetBrowserLoginInformation()
        {
            return _loginInfo;
        }

        private Guid GetImpersonatingUser(CrmService service)
        {
            var queryResult = service.RetrieveMultiple(new QueryExpression(SystemUser.EntityLogicalName)
            {
                NoLock = true,
                TopCount = 1,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Fields.UserName, ConditionOperator.Equal, _loginInfo.Username)
                    }
                }
            });
            if (queryResult.Entities.Count == 0)
                throw new TestExecutionException(Constants.ErrorCodes.USER_NOT_FOUND);
            return queryResult.Entities[0].Id;
        }
    }
}
