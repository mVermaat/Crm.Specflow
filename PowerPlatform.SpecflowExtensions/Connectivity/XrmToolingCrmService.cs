using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using PowerPlatform.SpecflowExtensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public class XrmToolingCrmService : CrmService
    {
        private readonly string _connectionString;
        private Guid? _userId;
        private UserSettings _userSettings;
        
        public override Guid UserId 
        {
            get 
            {
                if(!_userId.HasValue)
                {
                    _userId = GetUserId();
                }
                return _userId.Value;
            }
            set
            {
                _userId = value;
                ((CrmServiceClient)Service).CallerId = value;
                _userSettings = null;
            }
        }

        public override UserSettings UserSettings 
        { 
            get
            {
                if (_userSettings == null)
                    _userSettings = GetUserSettings(UserId);

                return _userSettings;
            }       
        }

        public XrmToolingCrmService(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override IOrganizationService ConnectToCrm()
        {
            Logger.WriteLine("Connecting to Dynamics CRM API");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new CrmServiceClient(_connectionString);

            if (!client.IsReady)
                throw new TestExecutionException(Constants.ErrorCodes.UNABLE_TO_LOGIN, client.LastCrmException, client.LastCrmError);

            return client;
        }

        private Guid GetUserId()
        {
            return Execute<WhoAmIResponse>(new WhoAmIRequest()).UserId;
        }
    }
}
