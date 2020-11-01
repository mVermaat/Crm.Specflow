using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.Interfaces;
using PowerPlatform.SpecflowExtensions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions
{
    public class RecordBuilder
    {
        private readonly ICrmContext _crmContext;
        private Dictionary<string, DefaultDataField[]> _defaultData;

        public RecordBuilder(ICrmContext context)
        {
            _crmContext = context;
            _defaultData = ReadDefaultData();
        }

        private Dictionary<string, DefaultDataField[]> ReadDefaultData()
        {
            string defaultDataFile = HelperMethods.GetAppSettingsValue("DefaultDataFile", true);

            if (string.IsNullOrEmpty(defaultDataFile))
                return new Dictionary<string, DefaultDataField[]>();

            XmlSerializer ser = new XmlSerializer(typeof(DefaultData));

            FileInfo dllPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
            FileInfo file = new FileInfo(string.Format(@"{0}\{1}", dllPath.DirectoryName, defaultDataFile));

            using (XmlReader reader = XmlReader.Create(file.FullName))
            {
                DefaultData data = ser.Deserialize(reader) as DefaultData;

                if (data == null || data.Entities == null || data.Entities.Length == 0)
                {
                    Logger.WriteLine("DefaultData is empty");
                    return new Dictionary<string, DefaultDataField[]>();
                }

                return data.Entities.ToDictionary(e => e.Name, e => e.Fields);
            }

        }

        public Table AddDefaultsToTable(string entityName, Table customFields)
        {
            Table result = new Table(customFields.Header.ToArray());

            Dictionary<string, TableRow> rows = new Dictionary<string, TableRow>();

            if (_defaultData.ContainsKey(entityName))
            {
                for (int i = 0; i < _defaultData[entityName].Length; i++)
                {
                    result.AddRow(_defaultData[entityName][i].Name, _defaultData[entityName][i].Value);
                    rows.Add(_defaultData[entityName][i].Name, result.Rows[i]);
                }
            }

            foreach (TableRow row in customFields.Rows)
            {
                if (rows.TryGetValue(row[Constants.SpecFlow.TABLE_KEY], out TableRow rowToUpdate))
                {
                    rowToUpdate[Constants.SpecFlow.TABLE_VALUE] = row[Constants.SpecFlow.TABLE_VALUE];
                }
                else
                {
                    result.AddRow(row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE]);
                }
            }

            return result;
        }

        public Entity SetupEntityWithDefaults(string entityName, Table customFields)
        {
            Entity toCreate = new Entity(entityName);
            AddDefaultsFromDefaultData(toCreate);
            AddDefaultsFromTable(toCreate, customFields);
            return toCreate;
        }

        public Entity SetupEntityFromParent(EntityReference parentEntity, string childEntityName, Table customFields, DataCollection<Entity> attributeMaps)
        {
            Dictionary<string, AttributeMetadata> parentMetadata = GlobalContext.Metadata.GetEntityMetadata(parentEntity.LogicalName).Attributes.ToDictionary(a => a.LogicalName);
            Dictionary<string, AttributeMetadata> childMetadata = GlobalContext.Metadata.GetEntityMetadata(childEntityName).Attributes.ToDictionary(a => a.LogicalName);

            IEnumerable<Entity> validMaps = attributeMaps.Where(m =>
            AttributeValid(parentMetadata, m.GetAttributeValue<string>(AttributeMap.Fields.SourceAttributeName)) &&
            AttributeValid(childMetadata, m.GetAttributeValue<string>(AttributeMap.Fields.TargetAttributeName)));

            Entity parentRecord = GlobalContext.ConnectionManager.CurrentCrmService.Retrieve(parentEntity,
                new ColumnSet(validMaps.Select(m => m.GetAttributeValue<string>(AttributeMap.Fields.SourceAttributeName)).ToArray()));

            Entity toCreate = new Entity(childEntityName);
            foreach (Entity map in validMaps)
            {
                if (parentRecord.Attributes.ContainsKey(map.GetAttributeValue<string>(AttributeMap.Fields.SourceAttributeName)))
                {
                    var sourceAttribute = map.GetAttributeValue<string>(AttributeMap.Fields.SourceAttributeName);

                    if (parentMetadata[sourceAttribute].IsPrimaryId.GetValueOrDefault())
                    {
                        toCreate[map.GetAttributeValue<string>(AttributeMap.Fields.TargetAttributeName)] = parentEntity;
                    }
                    else
                    {
                        toCreate[map.GetAttributeValue<string>(AttributeMap.Fields.TargetAttributeName)] = parentRecord[sourceAttribute];
                    }
                }
            }

            AddDefaultsFromTable(toCreate, customFields);

            return toCreate;
        }

        private bool AttributeValid(Dictionary<string, AttributeMetadata> childMetadata, string attributeName)
        {
            if (!childMetadata.TryGetValue(attributeName, out AttributeMetadata metadata))
            {
                return false;
            }

            return string.IsNullOrEmpty(metadata.AttributeOf);
        }

        private void AddDefaultsFromDefaultData(Entity toCreate)
        {
            if (_defaultData.ContainsKey(toCreate.LogicalName))
            {
                foreach (DefaultDataField defaultDataField in _defaultData[toCreate.LogicalName])
                {
                    toCreate[defaultDataField.Name] = ObjectConverter.ToCrmObject(toCreate.LogicalName, defaultDataField.Name, defaultDataField.Value, _crmContext);
                }
            }
        }

        private void AddDefaultsFromTable(Entity toCreate, Table customFields)
        {
            foreach (TableRow row in customFields.Rows)
            {
                toCreate[row[Constants.SpecFlow.TABLE_KEY]] = ObjectConverter.ToCrmObject(toCreate.LogicalName, row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE], _crmContext);
            }
        }

    }
}
