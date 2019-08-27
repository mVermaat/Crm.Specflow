using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public class FormLoadConditions
    {
        public string EntityName { get; set; }
        public Guid? RecordId { get; set; }

        public FormLoadConditions()
        {

        }
    }
}
