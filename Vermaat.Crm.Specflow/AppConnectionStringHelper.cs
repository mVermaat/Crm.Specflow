using System.Text;

namespace Vermaat.Crm.Specflow
{
    public class AppConnectionStringHelper : IConnectionStringHelper
    {
        public string GetConnectionString()
        {
            var authType = HelperMethods.GetAppSettingsValue("AuthType", true);
            var clientId = HelperMethods.GetAppSettingsValue("ClientId", true);
            var clientSecret = HelperMethods.GetAppSettingsValue("ClientSecret", true);
            var url = HelperMethods.GetAppSettingsValue("Url", true);

            return ToCrmClientString(authType, url, clientId, clientSecret);
        }

        public bool IsValid()
        { 
            var clientId = HelperMethods.GetAppSettingsValue("ClientId", true);
            var clientSecret = HelperMethods.GetAppSettingsValue("ClientSecret", true);
            return !string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret);
        }

        private string ToCrmClientString(string authType, string url, string clientId, string clientSecret)
        {
            var builder = new StringBuilder($"AuthType={authType};Url={url};RequireNewInstance=True");

            if (!string.IsNullOrWhiteSpace(clientId))
                builder.Append($";ClientId={clientId}");
            if (!string.IsNullOrWhiteSpace(clientSecret))
                builder.Append($";ClientSecret={clientSecret}");

            return builder.ToString();
        }
    }
}
