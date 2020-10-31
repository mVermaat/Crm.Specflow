using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public class ImpersonatedClientSecretCrmConnection : CrmConnection
    {
        private readonly string _clientId;
        private readonly SecureString _clientSecret;
        private readonly string _username;
        private readonly SecureString _password;

        public ImpersonatedClientSecretCrmConnection(string clientId, SecureString clientSecret,
            string username, SecureString password) : base($"ICS_{clientId}_{username}")
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _username = username;
            _password = password;
        }

        public override BrowserLoginDetails BrowserLoginDetails => new BrowserLoginDetails { Username = _username, Password = _password };

        public static ImpersonatedClientSecretCrmConnection FromAppConfig()
        {
            return new ImpersonatedClientSecretCrmConnection(
                HelperMethods.GetAppSettingsValue(Constants.AppSettings.CLIENT_ID, false),
                HelperMethods.GetAppSettingsValue(Constants.AppSettings.CLIENT_SECRET, false).ToSecureString(),
                HelperMethods.GetAppSettingsValue(Constants.AppSettings.USERNAME, false),
                HelperMethods.GetAppSettingsValue(Constants.AppSettings.PASSWORD, false).ToSecureString());
        }

        public static ImpersonatedClientSecretCrmConnection AdminConnectionFromAppConfig()
        {
            var clientId = HelperMethods.GetAppSettingsValue(Constants.AppSettings.ADMIN_CLIENT_ID, true) ?? HelperMethods.GetAppSettingsValue(Constants.AppSettings.CLIENT_ID, false);
            var clientSecret = HelperMethods.GetAppSettingsValue(Constants.AppSettings.ADMIN_CLIENT_SECRET, true)?.ToSecureString() ?? HelperMethods.GetAppSettingsValue(Constants.AppSettings.CLIENT_SECRET, false).ToSecureString();
            var username = HelperMethods.GetAppSettingsValue(Constants.AppSettings.ADMIN_USERNAME, true) ?? HelperMethods.GetAppSettingsValue(Constants.AppSettings.USERNAME);
            var password = HelperMethods.GetAppSettingsValue(Constants.AppSettings.ADMIN_PASSWORD, true)?.ToSecureString() ?? HelperMethods.GetAppSettingsValue(Constants.AppSettings.PASSWORD).ToSecureString();

            return new ImpersonatedClientSecretCrmConnection(clientId, clientSecret, username, password);
        }


        protected override ICrmService CreateServiceInstance()
        {
            var service = new XrmToolingCrmService($"AuthType=ClientSecret;Url={Url};ClientId={_clientId};ClientSecret={_clientSecret.ToUnsecureString()};RequireNewInstance=True");
            service.CallerId = GetImpersonatingUser(service);
            return service;
        }

        private Guid GetImpersonatingUser(ICrmService service)
        {
            var queryResult = service.RetrieveMultiple(new QueryExpression(SystemUser.EntityLogicalName)
            {
                NoLock = true,
                TopCount = 1,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUser.Fields.UserName, ConditionOperator.Equal, _username)
                    }
                }
            });
            if (queryResult.Entities.Count == 0)
                throw new TestExecutionException(Constants.ErrorCodes.USER_NOT_FOUND);
            return queryResult.Entities[0].Id;
        }
    }
}
