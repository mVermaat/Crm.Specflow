using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow
{
    public class ConnectionManager : IDisposable
    {
        private class ConnectionCache
        {
            public CrmService Service { get; set; }
            public UserDetails UserDetails { get; set; }
        }


        private readonly string _authType;
        private readonly Dictionary<string, ConnectionCache> _connectionCache;

        public CrmService CurrentConnection { get; private set; }
        public CrmService AdminConnection { get; private set; }
        public UserDetails CurrentUserDetails { get; private set; }
        public Uri Url { get; }

        public ConnectionManager()
        {
            Url = new Uri(HelperMethods.GetAppSettingsValue("Url"));
            _authType = HelperMethods.GetAppSettingsValue("AuthType");
            _connectionCache = new Dictionary<string, ConnectionCache>();
        }

        public void SetAdminConnection(UserDetails userDetails)
        {
            ConnectionCache connectionCache = GetConnectionCache(userDetails, "admin");
            AdminConnection = connectionCache.Service;
        }

        public void SetCurrentConnection(UserDetails userDetails)
        {
            ConnectionCache connectionCache = GetConnectionCache(userDetails, "current");
            CurrentConnection = connectionCache.Service;
            CurrentUserDetails = connectionCache.UserDetails;
        }

        private ConnectionCache GetConnectionCache(UserDetails userDetails, string connectionType)
        {
            Logger.WriteLine($"Changing {connectionType} connection to {userDetails.Username}");
            if (!_connectionCache.TryGetValue(userDetails.Username, out ConnectionCache connectionCache))
            {
                Logger.WriteLine("Connection doesn't exist. Creating new API connection");
                connectionCache = new ConnectionCache()
                {
                    Service = new CrmService(ToCrmClientString(userDetails)),
                    UserDetails = new UserDetails { Username = userDetails.Username, Password = userDetails.Password }
                };
                connectionCache.UserDetails.UserSettings = UserSettings.GetUserSettings(connectionCache.Service);
                _connectionCache.Add(userDetails.Username, connectionCache);

            }

            return connectionCache;
        }

        private string ToCrmClientString(UserDetails userDetails)
        {
            var builder = new StringBuilder($"AuthType={_authType};Url={Url};RequireNewInstance=True");

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
                    _connectionCache.Clear();
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
