using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Entities
{
    internal class Party
    {
        public EntityReference ConnectedParty { get; set; }
        public string EmailAddress { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(EmailAddress) ? ConnectedParty?.Id.ToString() : EmailAddress;
        }

        public static Party[] FromEntityCollection(EntityCollection col)
        {
            if (col == null || col.Entities == null || col.Entities.Count == 0)
                return Array.Empty<Party>();

            string entityName = col.EntityName ?? col.Entities[0]?.LogicalName;
            if (!string.IsNullOrEmpty(entityName) && entityName.Equals("activityparty"))
            {
                return col.Entities.Select(e => new Party()
                {
                    ConnectedParty = e.GetAttributeValue<EntityReference>("partyid"),
                    EmailAddress = e.GetAttributeValue<string>("addressused")
                }).ToArray();
            }
            else
            {
                return col.Entities.Select(e => new Party() { ConnectedParty = e.ToEntityReference() }).ToArray();
            }

        }
    }
}
