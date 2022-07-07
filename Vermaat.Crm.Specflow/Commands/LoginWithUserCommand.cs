using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Connectivity;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Commands
{
    public class LoginWithUserCommand : ApiOnlyCommand
    {
        private readonly UserProfile _userProfile;

        public LoginWithUserCommand(CrmTestingContext crmContext, UserProfile userProfile, string userAlias) 
            : base(crmContext)
        {
            _userProfile = userProfile;
        }

        public override void Execute()
        {
            var kvUri = new Uri($"https://{HelperMethods.GetAppSettingsValue("KeyVaultName", false)}.vault.azure.net");

            var client = new SecretClient(kvUri, new DefaultAzureCredential(new DefaultAzureCredentialOptions()
            {
                ExcludeEnvironmentCredential = true,
                ExcludeManagedIdentityCredential = true,
                ExcludeInteractiveBrowserCredential = true,
            }));

            var secretName = _userProfile.SecretName ?? _userProfile.Profile;
            Logger.WriteLine($"Getting secret {secretName}");
            var secret = client.GetSecret(secretName);

            GlobalTestingContext.ConnectionManager.SetCurrentConnection(new OAuthCrmConnection(_userProfile.Username, secret.Value.Value));
            
            Logger.WriteLine($"Successfully logged in with {_userProfile.Profile}");
        }
    }
}
