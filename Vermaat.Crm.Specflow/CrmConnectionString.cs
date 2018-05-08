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
            Username = HelperMethods.GetAppSettingsValue("Username");
            Password = HelperMethods.GetAppSettingsValue("Password");
            AuthType = HelperMethods.GetAppSettingsValue("AuthType");
        }

        public string ToCrmClientString()
        {
            return string.Format("AuthType={0};Username={1};Password={2};Url={3}", AuthType, Username, Password, Url);
        }
    }
}
