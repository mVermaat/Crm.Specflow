using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Entities
{
    public class UserAccessData
    {
        public bool HasReadAccess { get; set; }
        public bool HasWriteAccess { get; set; }
        public bool HasCreateAccess { get; set; }
        public bool HasDeleteAccess { get; set; }
        public bool HasAppendAccess { get; set; }
        public bool HasAppendToAccess { get; set; }
        public bool HasAssignAccess { get; set; }
        public bool HasShareAccess { get; set; }
    }
}
