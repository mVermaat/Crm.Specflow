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

        public CrmConnection(string identifier)
        {
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

        protected abstract ICrmService CreateServiceInstance();

    }
}
