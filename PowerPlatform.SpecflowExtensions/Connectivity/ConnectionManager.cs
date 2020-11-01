using BoDi;
using PowerPlatform.SpecflowExtensions.EasyRepro;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public class ConnectionManager : IDisposable
    {
        private readonly Dictionary<string, ICrmService> _connectionCache = new Dictionary<string, ICrmService>();
        private readonly BrowserSessionManager _browserSessionManager = new BrowserSessionManager();

        public ICrmConnection CurrentConnection { get; private set; }
        public ICrmConnection AdminConnection { get; private set; }
        public ICrmService CurrentCrmService => CurrentConnection.Service;
        public ICrmService AdminCrmService => AdminConnection.Service;

        public BrowserSession GetCurrentBrowserSession(ISeleniumContext seleniumContext)
        {
            var session = _browserSessionManager.GetBrowserSession(seleniumContext.BrowserOptions, CurrentConnection);
            return session;
        }

        public void SetAdminConnection(ICrmConnection connection)
        {
            Logger.WriteLine($"Changing admin connection to {connection.Identifier}");
            TryGetServiceFromCache(connection);
            AdminConnection = connection;
        }

        public void SetCurrentConnection(ICrmConnection connection)
        {
            Logger.WriteLine($"Changing current connection to {connection.Identifier}");
            TryGetServiceFromCache(connection);
            CurrentConnection = connection;
        }

        private void TryGetServiceFromCache(ICrmConnection connection)
        {
            if (_connectionCache.TryGetValue(connection.Identifier, out ICrmService service))
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
                
                if (disposing)
                {
                    Logger.WriteLine("Cleaning up CRM API sessions");
                    _connectionCache.Clear();

                    _browserSessionManager.Dispose();
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
