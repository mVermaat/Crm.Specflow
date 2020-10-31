using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Models
{
    internal class Lead
    {
        public static class Fields
        {
            public const string TransactionCurrencyId = "transactioncurrencyid";
            public const string CustomerId = "customerid";
            public const string CampaignId = "campaignid";
        }
    }

    internal enum Lead_StatusCode
    {
        Canceled = 7,
        CannotContact = 5,
        Contacted = 2,
        Lost = 4,
        New = 1,
        NoLongerInterested = 6,
        Qualified = 3,
    }
}
