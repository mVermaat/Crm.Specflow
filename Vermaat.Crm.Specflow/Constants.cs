using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public static class Constants
    {
        public static class SpecFlow
        {
            public const string TABLE_KEY = "Property";
            public const string TABLE_VALUE = "Value";
        }

        public class EntityNames
        {
            public const string SITEMAP = "sitemap";
            public const string SYSTEMUSER = "systemuser";
            public const string TEAM = "team";
        }

        public class General
        {
            public const string STATECODE = "statecode";
            public const string STATUSCODE = "statuscode";
            public const string CURRENCY = "transactioncurrencyid";
            public const string OWNERID = "ownerid";
        }

        public class Lead : General
        {
            public const string CUSTOMER = "customerid";
            public const string SOURCE_CAMPAIGN = "campaignid";

            public const int QUALIFIED_VALUE = 3;
        }

        public class SiteMap : General
        {
            public const string XML = "sitemapxml";
        }

        public class SystemUser : General
        {
            public const string USERNAME = "";
        }

        
    }
}
