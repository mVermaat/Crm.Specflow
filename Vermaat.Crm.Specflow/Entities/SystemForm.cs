using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Vermaat.Crm.Specflow.Connectivity;
using Vermaat.Crm.Specflow.Xml;

namespace Vermaat.Crm.Specflow.Entities
{
    internal class SystemForm
    {
        private static class Fields
        {
            internal const string FormXml = "formxml";
            internal const string Name = "name";
            internal const string ObjectTypeCode = "objecttypecode";
        }

        private const string EntityLogicalName = "systemform";

        private readonly Entity _record;
        private readonly Lazy<FormXmlDefinition> _parsedFormXml;

        public FormXmlDefinition FormXml => _parsedFormXml.Value;
        public Guid Id => _record.Id;
        public string Name => _record.GetAttributeValue<string>(Fields.Name);
        public string ObjectTypeCode => _record.GetAttributeValue<string>(Fields.ObjectTypeCode);

        public SystemForm(Entity record)
        {
            _record = record;
            _parsedFormXml = new Lazy<FormXmlDefinition>(ParseFormXml);
        }

        private FormXmlDefinition ParseFormXml()
        {
            var serializer = new XmlSerializer(typeof(FormXmlDefinition));
            using (var reader = new StringReader(_record.GetAttributeValue<string>(Fields.FormXml)))
            {
                return serializer.Deserialize(reader) as FormXmlDefinition;
            }
        }

        public static SystemForm GetSystemForm(CrmService service, string name, string objectTypeCode)
        {
            var result = service.RetrieveMultiple(new QueryExpression(EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Fields.FormXml, Fields.Name, Fields.ObjectTypeCode),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Fields.Name, ConditionOperator.Equal, name),
                        new ConditionExpression(Fields.ObjectTypeCode, ConditionOperator.Equal, objectTypeCode)
                    }
                },
                TopCount = 1
            }).Entities.FirstOrDefault();

            return result != null ? new SystemForm(result) : null;
        }

        internal static SystemForm GetById(CrmService service, Guid formId)
        {
            return new SystemForm(service.Retrieve("systemform", formId,
                new ColumnSet(Fields.FormXml, Fields.Name, Fields.ObjectTypeCode)));
        }
    }
}
