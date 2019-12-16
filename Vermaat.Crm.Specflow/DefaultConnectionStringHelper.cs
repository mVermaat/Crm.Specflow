using System.Text;

namespace Vermaat.Crm.Specflow
{
    public class DefaultConnectionStringHelper : IConnectionStringHelper
    {
        public string GetConnectionString()
        {
            var authType = HelperMethods.GetAppSettingsValue("AuthType", true);
            var userName = HelperMethods.GetAppSettingsValue("Username", true);
            var password = HelperMethods.GetAppSettingsValue("Password", true);
            var url = HelperMethods.GetAppSettingsValue("Url", true);

            return ToCrmClientString(authType, url, userName, password);
        }

        public bool IsValid()
        {
            var userName = HelperMethods.GetAppSettingsValue("Username", true);
            var password = HelperMethods.GetAppSettingsValue("Password", true);
            return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password);
        }

        private string ToCrmClientString(string authType, string url, string userName, string password)
        {
            var builder = new StringBuilder($"AuthType={authType};Url={url};RequireNewInstance=True");

            if (!string.IsNullOrWhiteSpace(userName))
                builder.Append($";Username={userName}");
            if (!string.IsNullOrWhiteSpace(password))
                builder.Append($";Password={password}");

            return builder.ToString();
        }
    }
}