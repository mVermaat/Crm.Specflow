using System.Collections.Generic;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    static class CompositeFields
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _compositeData;

        static CompositeFields()
        {
            _compositeData = new Dictionary<string, Dictionary<string, string>>()
            {
                { "lead", new Dictionary<string, string>()
                    {
                        { "firstname", "fullname" },
                        { "lastname", "fullname" }
                    }
                },
                { "contact", new Dictionary<string, string>()
                    {
                        { "firstname", "fullname" },
                        { "lastname", "fullname" }
                    }
                }
            };
        }

        public static string GetCompositeParentField(string entityName, string fieldName)
        {
            if (_compositeData.TryGetValue(entityName, out Dictionary<string, string> fieldDic) && 
                fieldDic.TryGetValue(fieldName, out string parentField))
                return parentField;
            else
                return null;
        }
    }
}
