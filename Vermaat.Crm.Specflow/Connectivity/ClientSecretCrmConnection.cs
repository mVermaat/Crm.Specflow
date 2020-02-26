using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Connectivity
{
    public class ClientSecretCrmConnection : CrmConnection
    {
        private readonly string _authType;
        private readonly string _url;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public ClientSecretCrmConnection(string clientId, string clientSecret)
            :base(clientId)
        {
            _authType = HelperMethods.GetAppSettingsValue("AuthType", false);
            _url = HelperMethods.GetAppSettingsValue("Url", false);
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public static ClientSecretCrmConnection CreateFromAppConfig()
        {
            return new ClientSecretCrmConnection(
                HelperMethods.GetAppSettingsValue("ClientId", false),
                HelperMethods.GetAppSettingsValue("ClientSecret", false));
        }

        public override CrmService CreateCrmServiceInstance()
        {
            return new CrmService($"AuthType={_authType};Url={_url};ClientId={Identifier};ClientSecret={_clientSecret};RequireNewInstance=True");
        }

        public override BrowserLoginDetails GetBrowserLoginInformation()
        {
            throw new TestExecutionException(Constants.ErrorCodes.APPLICATIONUSER_CANNOT_LOGIN);
        }
    }
}
