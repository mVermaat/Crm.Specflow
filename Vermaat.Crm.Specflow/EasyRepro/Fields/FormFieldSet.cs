using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    public class FormFieldSet
    {
        private List<FormField> _formFields;

        public FormFieldSet()
        {
            _formFields = new List<FormField>();
        }

        public void Add(FormField field)
        {
            _formFields.Add(field);
        }

        public FormField Get()
        {
            return _formFields.First();
        }

        public FormField Get(string tab)
        {
            throw new NotImplementedException();
        }
    }
}
