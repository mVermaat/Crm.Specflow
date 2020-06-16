using PowerPlatform.SpecflowExtensions.Connectivity;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions
{
    public static class GlobalContext
    {
        static GlobalContext()
        {
            ConnectionManager = new ConnectionManager();
            Metadata = new MetadataCache();
        }

        public static ConnectionManager ConnectionManager { get; }
        public static MetadataCache Metadata { get; }
        public static ErrorCodes ErrorCodes { get; }
    }
}

