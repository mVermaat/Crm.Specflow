using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Connectivity
{
    internal class CrmConnectionFactory
    {
        public static CrmConnection CreateNewConnection(UserProfile userProfile)
        {
            var kvUri = new Uri($"https://{HelperMethods.GetAppSettingsValue("KeyVaultName", false)}.vault.azure.net");
            var client = new SecretClient(kvUri, GetCredential());

            var secretName = userProfile.SecretName ?? userProfile.Profile;
            Logger.WriteLine($"Getting secret {secretName}");
            var secret = client.GetSecret(secretName);

            if (userProfile.MFA)
            {
                return new HybridCrmConnection(
                        HelperMethods.GetAppSettingsValue("ClientId", false),
                        HelperMethods.GetAppSettingsValue("ClientSecret", false),
                        userProfile.Username,
                        secret.Value.Value,
                        GetMFAKey(client, $"{secretName}MFA"));
            }
            else
            {
                return new OAuthCrmConnection(userProfile.Username, secret.Value.Value);
            }
        }

        private static TokenCredential GetCredential()
        {
            var clientId = HelperMethods.GetAppSettingsValue("KeyVaultClientId", true);
            if (!string.IsNullOrEmpty(clientId))
            {
                return new ClientSecretCredential(
                    HelperMethods.GetAppSettingsValue("TenantId", false),
                    clientId,
                    HelperMethods.GetAppSettingsValue("KeyVaultClientSecret", false));
            }

            clientId = HelperMethods.GetAppSettingsValue("ClientId", true);
            if (!string.IsNullOrEmpty(clientId))
            {
                return new ClientSecretCredential(
                    HelperMethods.GetAppSettingsValue("TenantId", false),
                    clientId,
                    HelperMethods.GetAppSettingsValue("ClientSecret", false));
            }

            return new DefaultAzureCredential(new DefaultAzureCredentialOptions()
            {
                ExcludeEnvironmentCredential = true,
                ExcludeManagedIdentityCredential = true,
                ExcludeInteractiveBrowserCredential = true,
            });
        }

        private static SecureString GetMFAKey(SecretClient client, string secretName)
        {
            var secret = client.GetSecret(secretName);

            return secret.Value.Value.ToSecureString();
        }
    }
}
