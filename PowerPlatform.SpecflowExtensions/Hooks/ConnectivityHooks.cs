using PowerPlatform.SpecflowExtensions.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Hooks
{
    [Binding]
    public class ConnectivityHooks
    {
        [BeforeScenario]
        public void SetDefaultConnection()
        {
            // Fallback to 'Default' for backwards compatibility
            var loginType = HelperMethods.GetAppSettingsValue(Constants.AppSettings.LOGIN_TYPE, true) ?? "Default";

            CrmConnection connection;
            CrmConnection adminConnection;
            switch (loginType)
            {
                case "Default":
                    connection = UsernamePasswordCrmConnection.FromAppConfig();
                    adminConnection = UsernamePasswordCrmConnection.AdminConnectionFromAppConfig();
                    break;
                //case "ClientSecret":
                //    connection = ClientSecretCrmConnection.CreateFromAppConfig();
                //    adminConnection = ClientSecretCrmConnection.CreateAdminConnectionFromAppConfig();
                //    break;
                //case "Hybrid":
                //    connection = HybridCrmConnection.CreateFromAppConfig();
                //    adminConnection = HybridCrmConnection.CreateAdminConnectionFromAppConfig();
                //    break;
                // Implementations can add their own 'LoginType'. If this is done, then this method shouldn't do anything
                default:
                    return;

            }

            GlobalContext.ConnectionManager.SetAdminConnection(adminConnection);
            GlobalContext.ConnectionManager.SetCurrentConnection(connection);

        }
    }
}
