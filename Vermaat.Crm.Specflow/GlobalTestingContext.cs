using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Connectivity;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow
{
    public static class GlobalTestingContext
    {
        public static ConnectionManager ConnectionManager { get; }
        public static MetadataCache Metadata { get; }
        public static LocalizedTexts ButtonTexts { get; }
        public static ErrorCodes ErrorCodes { get; }

        internal static BrowserManager BrowserManager { get; }

        static GlobalTestingContext()
        {
            ConnectionManager = new ConnectionManager();
            Metadata = new MetadataCache();
            ButtonTexts = new LocalizedTexts();
            BrowserManager = new BrowserManager(ButtonTexts);
            ErrorCodes = new ErrorCodes();
        }

    }
}
