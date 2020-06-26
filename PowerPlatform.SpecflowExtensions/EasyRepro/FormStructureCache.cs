using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro
{
    public class FormStructureCache
    {
        private Dictionary<string, Dictionary<Guid, FormStructure>> _cache;

        public FormStructureCache()
        {
            _cache = new Dictionary<string, Dictionary<Guid, FormStructure>>();
        }

        public FormStructure GetFormStructure(string entityName, Guid formId)
        {
            if(!_cache.TryGetValue(entityName, out var structureCache))
            {
                structureCache = new Dictionary<Guid, FormStructure>();
                _cache.Add(entityName, structureCache);
            }

            if (structureCache.TryGetValue(formId, out var structure))
                return structure;

            structure = new FormStructure();
            structureCache.Add(formId, structure);
            return structure;
        }        
    }
}
