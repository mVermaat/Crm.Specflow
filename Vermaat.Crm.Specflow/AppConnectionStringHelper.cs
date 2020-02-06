using System.Text;

namespace Vermaat.Crm.Specflow
{
    public class AppConnectionStringHelper : IConnectionStringHelper
    {
        public string GetConnectionString()
        {
            var authType = HelperMethods.GetAppSettingsValue("AuthType", false);
            var clientId = HelperMethods.GetAppSettingsValue("ClientId", false);
            var clientSecret = HelperMethods.GetAppSettingsValue("ClientSecret", false);
            var url = HelperMethods.GetAppSettingsValue("Url", false);

            return ToCrmClientString(authType, url, clientId, clientSecret);
        }

        public ValidationResult Validate()
        {
            // No additional validation is required
            return new ValidationResult();
        }

        private string ToCrmClientString(string authType, string url, string clientId, string clientSecret)
        {
            return $"AuthType={authType};Url={url};ClientId={clientId};ClientSecret={clientSecret};RequireNewInstance=True";
        }
    }
}
