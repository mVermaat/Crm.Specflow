using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    static class ExtensionMethods 
    {
        public static EntityReference ToEntityReference(this Entity entity, string primaryFieldAttribute)
        {
            return new EntityReference(entity.LogicalName, entity.Id) { Name = entity.GetAttributeValue<string>(primaryFieldAttribute) };
        }
    }
}
