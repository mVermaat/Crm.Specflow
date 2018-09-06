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
        public string Url { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string AuthType { get; private set; }

        public CrmConnectionString()
        {
            Url = HelperMethods.GetAppSettingsValue("Url");
            Username = HelperMethods.GetAppSettingsValue("Username", true);
            Password = HelperMethods.GetAppSettingsValue("Password", true);
            AuthType = HelperMethods.GetAppSettingsValue("AuthType");
        }

        public string ToCrmClientString()
        {
            var builder = new StringBuilder($"AuthType={AuthType};Url={Url}");

            if (!string.IsNullOrWhiteSpace(Username))
                builder.Append($";Username={Username}");
            if(!string.IsNullOrWhiteSpace(Password))
                builder.Append($";Password={Password}");

            return builder.ToString();
        }
    }
}
