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
       

        private ConnectionCache GetConnectionCache(IConnectionStringHelper connectionStringHelper, string connectionType)
        {
            Logger.WriteLine($"Changing {connectionType} connection to {connectionStringHelper.UserDetails.Username}");
            if (!_connectionCache.TryGetValue(connectionStringHelper.UserDetails.Username, out ConnectionCache connectionCache))
            {
                Logger.WriteLine("Connection doesn't exist. Creating new API connection");
                connectionCache = new ConnectionCache
                {
                    Service = new CrmService(connectionStringHelper.GetConnectionString()),
                    UserDetails = new UserDetails { Username = connectionStringHelper.UserDetails.Username, 
                        Password = connectionStringHelper.UserDetails.Password }
                };
                connectionCache.UserDetails.UserSettings = UserSettings.GetUserSettings(connectionCache.Service, connectionStringHelper.UserDetails.Username);
                _connectionCache.Add(connectionStringHelper.UserDetails.Username, connectionCache);
            }

            return connectionCache;
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                Logger.WriteLine("Cleaning up CRM API sessions");
                if (disposing)
                {
                    _connectionCache.Clear();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
