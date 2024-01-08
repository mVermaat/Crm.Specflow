namespace Vermaat.Crm.Specflow.Connectivity
{
    public abstract class CrmConnection
    {
        private CrmService _service;

        public CrmConnection(string identifier)
        {
            Identifier = $"{GetType().Name}_{identifier}";
        }

        public abstract CrmService CreateCrmServiceInstance();
        public abstract BrowserLoginDetails GetBrowserLoginInformation();

        public CrmService Service
        {
            get
            {
                if (_service == null)
                    _service = CreateCrmServiceInstance();
                return _service;
            }
            set { _service = value; }
        }

        public string Identifier { get; }
    }
}
