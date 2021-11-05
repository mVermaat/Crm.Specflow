using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Connectivity
{
    public class UsernamePasswordCrmConnection : CrmConnection
    {
        private readonly BrowserLoginDetails _loginInfo;

        public UsernamePasswordCrmConnection(string username, string password)
            : base(username)
        {
            _loginInfo = new BrowserLoginDetails
            {
                Username = username,
                Password = password.ToSecureString(),
                Url = HelperMethods.GetAppSettingsValue("Url", false)
            };
        }

        public static UsernamePasswordCrmConnection FromAppConfig()
        {
            return new UsernamePasswordCrmConnection(
                HelperMethods.GetAppSettingsValue("Username", false),
                HelperMethods.GetAppSettingsValue("Password", false));
        }

        public static UsernamePasswordCrmConnection AdminConnectionFromAppConfig()
        {
            var userName = HelperMethods.GetAppSettingsValue("AdminUsername", true) ?? HelperMethods.GetAppSettingsValue("Username");
            var password = HelperMethods.GetAppSettingsValue("AdminPassword", true) ?? HelperMethods.GetAppSettingsValue("Password");
            return new UsernamePasswordCrmConnection(userName, password);
        }


        public override CrmService CreateCrmServiceInstance()
        {
            return new CrmService($"AuthType=Office365;Url='{_loginInfo.Url}';Username='{_loginInfo.Username}';Password='{_loginInfo.Password.ToUnsecureString()}';RequireNewInstance=True");
        }

        public override BrowserLoginDetails GetBrowserLoginInformation()
        {
            return _loginInfo;
        }
    }
}
