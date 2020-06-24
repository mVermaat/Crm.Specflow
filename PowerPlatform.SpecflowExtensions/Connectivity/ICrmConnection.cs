using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Connectivity
{
    public interface ICrmConnection
    {
        string Identifier { get; }
        ICrmService Service { get; set; }
        BrowserLoginDetails BrowserLoginDetails { get; }

    }
}
