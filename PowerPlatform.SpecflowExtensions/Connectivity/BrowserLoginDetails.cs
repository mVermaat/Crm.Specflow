using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public class BrowserLoginDetails
    {
        public string Username { get; set; }
        public SecureString Password { get; set; }
    }
}
