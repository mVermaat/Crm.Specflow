using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    class Form : IForm
    {

        public Form(string entityName, Guid formId)
        {
            FormStructure = GlobalContext.FormStructureCache.GetFormStructure(entityName, formId);
        }

        public Form(FormStructure formStructure)
        {
            FormStructure = formStructure;
        }

        public FormStructure FormStructure { get; }
    }
}
