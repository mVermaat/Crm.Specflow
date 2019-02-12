using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    class FormData
    {
        private readonly CrmTestingContext _crmContext;
        private readonly string _entityName;
        private List<IFormField> _fields;

        public IEnumerable<IFormField> Fields => _fields;

        public FormData(CrmTestingContext crmContext, string entityName, Table fieldData)
        {
            _crmContext = crmContext;
            _entityName = entityName;
            _fields = GenerateFormFields(fieldData);
        }

        private List<IFormField> GenerateFormFields(Table fieldData)
        {
            List<IFormField> result = new List<IFormField>();
            var composites = new Dictionary<string, CompositeFormField>();

            foreach(var row in fieldData.Rows)
            {
                var formField = new FormField(_crmContext, _entityName, row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE]);
                string compositeParent = CompositeFields.GetCompositeParentField(_entityName, row[Constants.SpecFlow.TABLE_KEY]);

                if (!string.IsNullOrEmpty(compositeParent))
                {
                    if (!composites.TryGetValue(compositeParent, out CompositeFormField field))
                    {
                        field = new CompositeFormField(compositeParent);
                        composites.Add(compositeParent, field);
                        result.Add(field);
                    }
                    field.AddField(formField);
                }
                else
                {
                    result.Add(formField);
                }
            }
            return result;
        }
    }
}
