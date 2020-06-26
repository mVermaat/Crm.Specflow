using Microsoft.Dynamics365.UIAutomation.Browser;
using PowerPlatform.SpecflowExtensions.Connectivity;
using PowerPlatform.SpecflowExtensions.EasyRepro;
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
            ErrorCodes = new ErrorCodes();
            Url = new Uri(HelperMethods.GetAppSettingsValue(Constants.AppSettings.URL));
            FormStructureCache = new FormStructureCache();
        }

        public static ConnectionManager ConnectionManager { get; }
        public static MetadataCache Metadata { get; }
        public static ErrorCodes ErrorCodes { get; }
        public static FormStructureCache FormStructureCache { get; }

        public static Uri Url { get; }
    }
}

