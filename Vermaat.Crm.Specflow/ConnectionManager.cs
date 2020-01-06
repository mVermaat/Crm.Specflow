using System;
using System.Collections.Generic;
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

        private readonly Dictionary<string, ConnectionCache> _connectionCache;

        public CrmService CurrentConnection { get; private set; }
        public CrmService AdminConnection { get; private set; }
        public UserDetails CurrentUserDetails { get; private set; }
        public Uri Url { get; }

        public void SetAdminConnection(IConnectionStringHelper connectionStringHelper)
        {
            ConnectionCache connectionCache = GetConnectionCache(connectionStringHelper, "admin");
            AdminConnection = connectionCache.Service;
        }

        public void SetCurrentConnection(IConnectionStringHelper connectionStringHelper)
        {
            ConnectionCache connectionCache = GetConnectionCache(connectionStringHelper, "current");
            CurrentConnection = connectionCache.Service;
            CurrentUserDetails = connectionCache.UserDetails;
        }

        public ConnectionManager()
        {
            Url = new Uri(HelperMethods.GetAppSettingsValue("Url"));
            _connectionCache = new Dictionary<string, ConnectionCache>();
        }
        
        public UserDetails GetDetails()
        {
            return new UserDetails
            {
                Username = HelperMethods.GetAppSettingsValue("Username", true),
                Password = HelperMethods.GetAppSettingsValue("Password", true)
            };
        }

        private ConnectionCache GetConnectionCache(IConnectionStringHelper connectionStringHelper, string connectionType)
        {
            Logger.WriteLine($"Changing {connectionType} connection to {GetDetails().Username}");
            if (!_connectionCache.TryGetValue(GetDetails().Username, out ConnectionCache connectionCache))
            {
                Logger.WriteLine("Connection doesn't exist. Creating new API connection");
                connectionCache = new ConnectionCache
                {
                    Service = new CrmService(connectionStringHelper.GetConnectionString()),
                    UserDetails = new UserDetails { Username = GetDetails().Username, Password = GetDetails().Password }
                };
                connectionCache.UserDetails.UserSettings = UserSettings.GetUserSettings(connectionCache.Service, GetDetails().Username);
                _connectionCache.Add(GetDetails().Username, connectionCache);
            }

            return connectionCache;
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
