using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Connectivity
{
   public class DefaultCrmConnection : CrmConnection
    {
        private readonly string _authType;
        private readonly BrowserLoginDetails _loginInfo;

        public DefaultCrmConnection(string username, string password) 
            : base(username)
        {
            _authType = HelperMethods.GetAppSettingsValue("AuthType", false);
            _loginInfo = new BrowserLoginDetails
            {
                Username = username,
                Password = password.ToSecureString(),
                Url = HelperMethods.GetAppSettingsValue("Url", false)
            };
        }

        public static DefaultCrmConnection CreateFromAppConfig()
        {
            return new DefaultCrmConnection(
                HelperMethods.GetAppSettingsValue("Username", false),
                HelperMethods.GetAppSettingsValue("Password", false));            
        }


        public override CrmService CreateCrmServiceInstance()
        {
            return new CrmService($"AuthType={_authType};Url={_loginInfo.Url};Username={Identifier};Password={_loginInfo.Password.ToUnsecureString()};RequireNewInstance=True");
        }

        public override BrowserLoginDetails GetBrowserLoginInformation()
        {
            return _loginInfo;
        }
    }
}
