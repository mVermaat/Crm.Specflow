using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions
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
