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
        private BrowserLoginDetails _browserLoginDetails;

        public override BrowserLoginDetails BrowserLoginDetails => _browserLoginDetails;

        public UsernamePasswordCrmConnection(string username, SecureString password)
            : base(username)
        {
            _authType = HelperMethods.GetAppSettingsValue(Constants.AppSettings.AUTH_TYPE, false);
            _browserLoginDetails = new BrowserLoginDetails
            {
                Password = password,
                Username = username
            };
        }

        public static UsernamePasswordCrmConnection FromAppConfig()
        {
            return new UsernamePasswordCrmConnection(
                HelperMethods.GetAppSettingsValue(Constants.AppSettings.USERNAME, false),
                HelperMethods.GetAppSettingsValue(Constants.AppSettings.PASSWORD, false).ToSecureString());
        }

        public static UsernamePasswordCrmConnection AdminConnectionFromAppConfig()
        {
            var username = HelperMethods.GetAppSettingsValue(Constants.AppSettings.ADMIN_USERNAME, true) ?? HelperMethods.GetAppSettingsValue(Constants.AppSettings.USERNAME);
            var password = HelperMethods.GetAppSettingsValue(Constants.AppSettings.ADMIN_PASSWORD, true)?.ToSecureString() ?? HelperMethods.GetAppSettingsValue(Constants.AppSettings.PASSWORD).ToSecureString();
            return new UsernamePasswordCrmConnection(username, password);
        }

        protected override ICrmService CreateServiceInstance()
        {
            return new XrmToolingCrmService($"AuthType={_authType};Url={Url};Username={BrowserLoginDetails.Username};Password={BrowserLoginDetails.Password.ToUnsecureString()};RequireNewInstance=True");
        }
    }
}
