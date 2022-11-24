using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Entities
{
    public class FormXmlDefinition
    {
        public FormXmlRole[] DisplayConditions { get; set; }
    }

    public class FormXmlRole
    {
        public string Id { get; set; }
    }


}
