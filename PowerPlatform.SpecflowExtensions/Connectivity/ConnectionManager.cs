using PowerPlatform.SpecflowExtensions.EasyRepro;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public class ConnectionManager
    {
        private readonly Dictionary<string, ICrmService> _connectionCache = new Dictionary<string, ICrmService>();
        private readonly BrowserSessionManager _browserSessionManager = new BrowserSessionManager();
        private ICrmConnection _currentConnection;
        private ICrmConnection _adminConnection;

        public ICrmService CurrentConnection => _currentConnection.Service;
        public ICrmService AdminConnection => _adminConnection.Service;

        public BrowserSession GetCurrentBrowserSession(ISeleniumContext seleniumContext)
        {
            return _browserSessionManager.GetBrowserSession(seleniumContext.BrowserOptions, _currentConnection);
        }

        public void SetAdminConnection(ICrmConnection connection)
        {
            Logger.WriteLine($"Changing admin connection to {connection.Identifier}");
            TryGetServiceFromCache(connection);
            _adminConnection = connection;
        }

        public void SetCurrentConnection(ICrmConnection connection)
        {
            Logger.WriteLine($"Changing current connection to {connection.Identifier}");
            TryGetServiceFromCache(connection);
            _currentConnection = connection;
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
    }
}
