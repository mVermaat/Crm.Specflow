using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Connectivity
{
    public class HybridCrmConnection : CrmConnection
    {
        private readonly string _authType;
        private readonly BrowserLoginDetails _loginInfo;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public HybridCrmConnection(string clientId, string clientSecret, string browserUsername, string browserPassword)
            : base(browserUsername)
        {
            _authType = HelperMethods.GetAppSettingsValue("AuthType", false);
            _clientId = clientId;
            _clientSecret = clientSecret;
            _loginInfo = new BrowserLoginDetails
            {
                Url = HelperMethods.GetAppSettingsValue("Url", false),
                Username = browserUsername,
                Password = browserPassword.ToSecureString()
            };
        }

        public static HybridCrmConnection CreateFromAppConfig()
        {
            return new HybridCrmConnection(
                HelperMethods.GetAppSettingsValue("ClientId", false),
                HelperMethods.GetAppSettingsValue("ClientSecret", false),
                HelperMethods.GetAppSettingsValue("Username", false),
                HelperMethods.GetAppSettingsValue("Password", false));
        }

        public static HybridCrmConnection CreateAdminConnectionFromAppConfig()
        {
            var clientId = HelperMethods.GetAppSettingsValue("AdminClientId", true) ?? HelperMethods.GetAppSettingsValue("ClientId");
            var clientSecret = HelperMethods.GetAppSettingsValue("AdminClientSecret", true) ?? HelperMethods.GetAppSettingsValue("ClientSecret");
            var username = HelperMethods.GetAppSettingsValue("AdminUsername", true) ?? HelperMethods.GetAppSettingsValue("Username");
            var password = HelperMethods.GetAppSettingsValue("AdminPassword", true) ?? HelperMethods.GetAppSettingsValue("Password");
            return new HybridCrmConnection(clientId, clientSecret, username, password);
        }

        public override CrmService CreateCrmServiceInstance()
        {
            return new CrmService($"AuthType={_authType};Url={_loginInfo.Url};ClientId={_clientId};ClientSecret={_clientSecret};RequireNewInstance=True");
        }

        public override BrowserLoginDetails GetBrowserLoginInformation()
        {
            return _loginInfo;
        }
    }
}
