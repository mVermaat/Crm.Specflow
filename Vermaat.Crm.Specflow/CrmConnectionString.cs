using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public class CrmConnectionString
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthType { get; set; }
        public string AppName { get; set; }

        public static CrmConnectionString CreateFromAppConfig()
        {
            return new CrmConnectionString
            {
                Url = HelperMethods.GetAppSettingsValue("Url"),
                Username = HelperMethods.GetAppSettingsValue("Username", true),
                Password = HelperMethods.GetAppSettingsValue("Password", true),
                AuthType = HelperMethods.GetAppSettingsValue("AuthType"),
                AppName = HelperMethods.GetAppSettingsValue("AppName", true)
            };
        }

        public string ToCrmClientString()
        {
            var builder = new StringBuilder($"AuthType={AuthType};Url={Url}");

            if (!string.IsNullOrWhiteSpace(Username))
                builder.Append($";Username={Username}");
            if (!string.IsNullOrWhiteSpace(Password))
                builder.Append($";Password={Password}");

            return builder.ToString();
        }
    }
}
