namespace Vermaat.Crm.Specflow.Connectivity
{
    public abstract class CrmConnection
    {
        private CrmServiceBase _service;

        public CrmConnection(string identifier)
        {
            Identifier = $"{GetType().Name}_{identifier}";
        }

        public abstract CrmServiceBase CreateCrmServiceInstance();
        public abstract BrowserLoginDetails GetBrowserLoginInformation();

        public CrmServiceBase Service
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
