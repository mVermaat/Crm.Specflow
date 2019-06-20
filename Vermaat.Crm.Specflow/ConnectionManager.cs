using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public class ConnectionManager : IDisposable
    {
        private readonly string _authType;
        private readonly Dictionary<string, CrmService> _services;

        public CrmService CurrentConnection { get; private set; }
        public UserDetails CurrentUserDetails { get; private set; }
        public Uri Url { get; }

        public ConnectionManager()
        {
            Url = new Uri(HelperMethods.GetAppSettingsValue("Url"));
            _authType = HelperMethods.GetAppSettingsValue("AuthType");
            _services = new Dictionary<string, CrmService>();
        }

        public void SetCurrentConnection(UserDetails userDetails)
        {
            Logger.WriteLine($"Changing current connection to {userDetails.Username}");
            if(!_services.TryGetValue(userDetails.Username, out CrmService crmService))
            {
                Logger.WriteLine("Connection doesn't exist. Creating new API connection");
                crmService = new CrmService(ToCrmClientString(userDetails));
                _services.Add(userDetails.Username, crmService);
            }
            CurrentConnection = crmService;
            CurrentUserDetails = userDetails;
        }

        private string ToCrmClientString(UserDetails userDetails)
        {
            var builder = new StringBuilder($"AuthType={_authType};Url={Url}");

            if (!string.IsNullOrWhiteSpace(userDetails.Username))
                builder.Append($";Username={userDetails.Username}");
            if (!string.IsNullOrWhiteSpace(userDetails.Password))
                builder.Append($";Password={userDetails.Password}");

            return builder.ToString();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Logger.WriteLine("Cleaning up CRM API sessions");
                if (disposing)
                {
                    _services.Clear();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
