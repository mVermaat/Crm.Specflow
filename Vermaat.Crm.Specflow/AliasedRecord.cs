using Microsoft.Xrm.Sdk;
using System;

namespace Vermaat.Crm.Specflow
{
    class AliasedRecord
    {
        public DateTime CreatedOn { get; }
        public string Alias { get; }
        public EntityReference Record { get; set; }

        public AliasedRecord(string alias, EntityReference record)
        {
            Alias = alias;
            Record = record;
            CreatedOn = DateTime.Now;
        }
    }
}
