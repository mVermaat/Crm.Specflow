using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public class ClientSecretCrmConnection : CrmConnection
    {
        private readonly string _clientId;
        private readonly SecureString _clientSecret;

        public override BrowserLoginDetails BrowserLoginDetails => throw new TestExecutionException(Constants.ErrorCodes.APPLICATIONUSER_CANNOT_LOGIN);

        public ClientSecretCrmConnection(string clientId, SecureString clientSecret) 
            : base($"CS_{clientId}")
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }


        public static ClientSecretCrmConnection FromAppConfig()
        {
            return new ClientSecretCrmConnection(
                HelperMethods.GetAppSettingsValue(Constants.AppSettings.CLIENT_ID, false),
                HelperMethods.GetAppSettingsValue(Constants.AppSettings.CLIENT_SECRET, false).ToSecureString());
        }

        public static ClientSecretCrmConnection AdminConnectionFromAppConfig()
        {
            var clientId = HelperMethods.GetAppSettingsValue(Constants.AppSettings.ADMIN_CLIENT_ID, true) ?? HelperMethods.GetAppSettingsValue(Constants.AppSettings.CLIENT_ID, false);
            var clientSecret = HelperMethods.GetAppSettingsValue(Constants.AppSettings.ADMIN_CLIENT_SECRET, true)?.ToSecureString() ?? HelperMethods.GetAppSettingsValue(Constants.AppSettings.CLIENT_SECRET, false).ToSecureString();
            return new ClientSecretCrmConnection(clientId, clientSecret);
        }

        protected override ICrmService CreateServiceInstance()
        {
            return new XrmToolingCrmService($"AuthType=ClientSecret;Url={Url};ClientId={_clientId};ClientSecret={_clientSecret.ToUnsecureString()};RequireNewInstance=True");
        }
    }
}
