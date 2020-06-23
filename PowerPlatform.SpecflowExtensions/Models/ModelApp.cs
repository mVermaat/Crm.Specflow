using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Models
{
    public class ModelApp
    {
        internal const string EntityLogicalName = "appmodule";
        private readonly Entity _entity;

        internal static class Fields
        {
            internal const string UniqueName = "uniquename";
        }

        public ModelApp(Entity modelAppEntity)
        {
            _entity = modelAppEntity;
        }


        public string Name => _entity.GetAttributeValue<string>(Fields.UniqueName);
        public Guid Id => _entity.Id;
    }
}
