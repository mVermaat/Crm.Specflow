using System.Text;

namespace Vermaat.Crm.Specflow
{
    public class DefaultConnectionStringHelper : IConnectionStringHelper
    {
        public string GetConnectionString()
        {
            var authType = HelperMethods.GetAppSettingsValue("AuthType", false);
            var userName = HelperMethods.GetAppSettingsValue("Username", true);
            var password = HelperMethods.GetAppSettingsValue("Password", true);
            var url = HelperMethods.GetAppSettingsValue("Url", false);

            return ToCrmClientString(authType, url, userName, password);
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            var authType = HelperMethods.GetAppSettingsValue("AuthType", false);
            var userName = HelperMethods.GetAppSettingsValue("Username", true);
            var password = HelperMethods.GetAppSettingsValue("Password", true);

            if(authType.Equals("Office365", System.StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(userName))
                    result.AddError("Username is required");
                if (string.IsNullOrEmpty(password))
                    result.AddError("Password is required");
            }

            return result;
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