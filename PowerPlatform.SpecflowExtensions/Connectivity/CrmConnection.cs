using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public abstract class CrmConnection : ICrmConnection
    {
        private readonly string _identifier;
        private ICrmService _service;
        
        
        public Uri Url { get; private set; }

        public CrmConnection(string identifier)
        {
            Url = new Uri(HelperMethods.GetAppSettingsValue(Constants.AppSettings.URL, false));
            _identifier = identifier;
        }

        public string Identifier => $"{GetType().Name}_{_identifier}";

        public ICrmService Service
        {
            get
            {
                if (_service == null)
                    _service = CreateServiceInstance();
                return _service;
            }
            set { _service = value; }
        }

        public abstract BrowserLoginDetails BrowserLoginDetails { get; }

        protected abstract ICrmService CreateServiceInstance();

    }
}
