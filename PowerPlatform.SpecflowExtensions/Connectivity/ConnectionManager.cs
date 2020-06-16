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

        public ICrmService CurrentConnection { get; private set; }

        public ICrmService AdminConnection { get; private set; }

        public void SetAdminConnection(ICrmConnection connection)
        {
            Logger.WriteLine($"Changing admin connection to {connection.Identifier}");
            TryGetServiceFromCache(connection);
            AdminConnection = connection.Service;
        }

        public void SetCurrentConnection(ICrmConnection connection)
        {
            Logger.WriteLine($"Changing current connection to {connection.Identifier}");
            TryGetServiceFromCache(connection);
            CurrentConnection = connection.Service;
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
