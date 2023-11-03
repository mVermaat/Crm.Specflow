using System.Security;

namespace Vermaat.Crm.Specflow.Connectivity
{
    public class BrowserLoginDetails
    {
        public string Username { get; set; }
        public SecureString Password { get; set; }
        public SecureString MfaKey { get; set; }
        public string Url { get; set; }

    }
}
