using System;
using System.Collections.Generic;

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
        internal CrmConnection CurrentConnectionObject => _currentConnection;

        public ConnectionManager()
        {
            _connectionCache = new Dictionary<string, CrmService>();
        }

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
                Logger.WriteLine($"{connection.Identifier} is already connected. Getting service from cache");
                connection.Service = service;
            }
            else
            {
                Logger.WriteLine($"{connection.Identifier} is not connected. Adding service to cache");
                _connectionCache.Add(connection.Identifier, connection.Service);
            }
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
