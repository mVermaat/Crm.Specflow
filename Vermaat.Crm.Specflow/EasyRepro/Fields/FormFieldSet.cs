using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    internal class FormFieldSet
    {
        private Dictionary<string, Dictionary<string, List<FormField>>> _formFields;

        public FormFieldSet()
        {
            _formFields = new Dictionary<string, Dictionary<string, List<FormField>>>();
        }

        public void Add(FormField field, string tab, string section)
        {
            var tabNotNull = tab ?? string.Empty;
            var sectionNotNull = section ?? string.Empty;

            if (!_formFields.TryGetValue(tabNotNull, out var sectionDic))
            {
                sectionDic = new Dictionary<string, List<FormField>>();
                _formFields.Add(tabNotNull, sectionDic);
            }

            if (!sectionDic.TryGetValue(sectionNotNull, out var fieldList))
            {
                fieldList = new List<FormField>();
                sectionDic.Add(sectionNotNull, fieldList);
            }

            fieldList.Add(field);
        }

        public FormField Get()
        {
            return _formFields.Values.First().Values.First().First();
        }

        public FormField Get(string tab, bool allowNull)
        {
            if (_formFields.TryGetValue(tab ?? string.Empty, out var sectionDic))
            {
                return sectionDic.Values.First().First();
            }

            if (allowNull)
                return null;
            else
                throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_IN_TAB, tab);
        }

        public FormField Get(string tab, string section, bool allowNull)
        {
            if (_formFields.TryGetValue(tab ?? string.Empty, out var sectionDic))
            {
                if (sectionDic.TryGetValue(section ?? string.Empty, out var fieldList))
                {
                    return fieldList.First();
                }
                else if (allowNull)
                    return null;
                else
                    throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_IN_SECTION, section);
            }
            else if (allowNull)
                return null;
            else
                throw new TestExecutionException(Constants.ErrorCodes.FIELD_NOT_IN_TAB, tab);

        }
    }
}
