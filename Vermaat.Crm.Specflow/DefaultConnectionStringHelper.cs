using System.Text;

namespace Vermaat.Crm.Specflow
{
    public class DefaultConnectionStringHelper : IConnectionStringHelper
    {
        public UserDetails UserDetails { get; }

        public DefaultConnectionStringHelper()
        {
            UserDetails = new UserDetails
            {
                Username = HelperMethods.GetAppSettingsValue("Username", true),
                Password = HelperMethods.GetAppSettingsValue("Password", true),
            };
        }

        public DefaultConnectionStringHelper(UserDetails userDetails)
        {
            UserDetails = userDetails;
        }

        public string GetConnectionString()
        {
            var authType = HelperMethods.GetAppSettingsValue("AuthType", false);
            
            var url = HelperMethods.GetAppSettingsValue("Url", false);

            return ToCrmClientString(authType, url);
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            var authType = HelperMethods.GetAppSettingsValue("AuthType", false);
         

            if(authType.Equals("Office365", System.StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(UserDetails.Username))
                    result.AddError("Username is required");
                if (string.IsNullOrEmpty(UserDetails.Password))
                    result.AddError("Password is required");
            }

            return result;
        }

        private string ToCrmClientString(string authType, string url)
        {
            var builder = new StringBuilder($"AuthType={authType};Url={url};RequireNewInstance=True");

            if (!string.IsNullOrWhiteSpace(UserDetails.Username))
                builder.Append($";Username={UserDetails.Username}");
            if (!string.IsNullOrWhiteSpace(UserDetails.Password))
                builder.Append($";Password={UserDetails.Password}");

            return builder.ToString();
        }
    }
}