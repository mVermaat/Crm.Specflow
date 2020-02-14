using System;
using System.Collections.Generic;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.Connectivity
{
    public class ConnectionManager : IDisposable
    {
        private readonly Dictionary<string, CrmService> _connectionCache;

        private CrmConnection _adminConnection;
        private CrmConnection _currentConnection;
        

        public CrmService AdminConnection => _adminConnection.Service;
        public CrmService CurrentConnection => _currentConnection.Service;
        public BrowserLoginDetails CurrentBrowserLoginDetails => _currentConnection.GetBrowserLoginInformation();
        public BrowserLoginDetails CurrentLoginDetails { get; private set; }



        public void SetAdminConnection(CrmConnection connection)
        {
            Logger.WriteLine($"Changing admin connection to {connection.Identifier}");
            TryGetServiceFromCache(connection);
            _adminConnection = connection;
        }

        
        public void SetCurrentConnection(CrmConnection connection)
        {
            Logger.WriteLine($"Changing current connection to {connection.Identifier}");
            TryGetServiceFromCache(connection);
            _currentConnection = connection;
        }

        private void TryGetServiceFromCache(CrmConnection connection)
        {
            if (_connectionCache.TryGetValue(connection.Identifier, out CrmService service))
            {
                Logger.WriteLine($"{connection.Identifier} dis already connected. Getting service from cache");
                connection.Service = service;
            }
        }


        //public CrmService CurrentConnection { get; private set; }
        //public CrmService AdminConnection { get; private set; }
        //public UserDetails CurrentUserDetails { get; private set; }
        //public Uri Url { get; }

        //public void SetAdminConnection(string identifier)
        //{
        //    ConnectionCache connectionCache = GetConnectionCache(connection, "admin");
        //    AdminConnection = connectionCache.Service;
        //}

        //public void SetCurrentConnection(string identifier)
        //{
        //    ConnectionCache connectionCache = GetConnectionCache(connection, "current");
        //    CurrentConnection = connectionCache.Service;
        //    CurrentUserDetails = connectionCache.UserDetails;
        //}

        //public ConnectionManager()
        //{
        //    Url = new Uri(HelperMethods.GetAppSettingsValue("Url"));
        //    _connectionCache = new Dictionary<string, ConnectionCache>();
        //}


        //private ConnectionCache GetConnectionCache(CrmConnection connection, string connectionType)
        //{
        //    Logger.WriteLine($"Changing {connectionType} connection to {connection.UserDetails.Username}");
        //    if (!_connectionCache.TryGetValue(connection.UserDetails.Username, out ConnectionCache connectionCache))
        //    {
        //        Logger.WriteLine("Connection doesn't exist. Creating new API connection");
        //        connectionCache = new ConnectionCache
        //        {
        //            Service = new CrmService(connection.GetConnectionString()),
        //            UserDetails = new UserDetails { Username = connection.UserDetails.Username, 
        //                Password = connection.UserDetails.Password }
        //        };
        //        connectionCache.UserDetails.UserSettings = UserSettings.GetUserSettings(connectionCache.Service, connection.UserDetails.Username);
        //        _connectionCache.Add(connection.UserDetails.Username, connectionCache);
        //    }

        //    return connectionCache;
        //}

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
