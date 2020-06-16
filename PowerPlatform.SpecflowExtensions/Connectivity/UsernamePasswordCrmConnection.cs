using Microsoft.Dynamics365.UIAutomation.Browser;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public class UsernamePasswordCrmConnection : CrmConnection
    {
        private readonly string _authType;
        private readonly string _username;
        private readonly SecureString _password;
        private readonly string _url;

        public UsernamePasswordCrmConnection(string username, SecureString password)
            : base(username)
        {
            _authType = HelperMethods.GetAppSettingsValue("AuthType", false);
            _username = username;
            _password = password;
            _url = HelperMethods.GetAppSettingsValue("Url", false);
            
        }

        public static UsernamePasswordCrmConnection FromAppConfig()
        {
            return new UsernamePasswordCrmConnection(
                HelperMethods.GetAppSettingsValue("Username", false),
                HelperMethods.GetAppSettingsValue("Password", false).ToSecureString());
        }

        public static UsernamePasswordCrmConnection AdminConnectionFromAppConfig()
        {
            var userName = HelperMethods.GetAppSettingsValue("AdminUsername", true) ?? HelperMethods.GetAppSettingsValue("Username");
            var password = HelperMethods.GetAppSettingsValue("AdminPassword", true).ToSecureString() ?? HelperMethods.GetAppSettingsValue("Password").ToSecureString();
            return new UsernamePasswordCrmConnection(userName, password);
        }


        protected override ICrmService CreateServiceInstance()
        {
            return new XrmToolingCrmService($"AuthType={_authType};Url={_url};Username={_username};Password={_password.ToUnsecureString()};RequireNewInstance=True");
        }
    }
}
