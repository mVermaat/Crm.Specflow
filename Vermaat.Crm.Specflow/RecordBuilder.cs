using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Xml;

namespace Vermaat.Crm.Specflow
{
    public class RecordBuilder
    {
        private readonly CrmTestingContext _crmContext;
        private Dictionary<string, DefaultDataField[]> _defaultData;

        public RecordBuilder(CrmTestingContext context)
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

                return data.Entities.ToDictionary(e => e.Name, e => e.Fields);
            }

        }

        public Table AddDefaultsToTable(string entityName, Table customFields)
        {
            Table result = new Table(customFields.Header.ToArray());

            Dictionary<string, TableRow> rows = new Dictionary<string, TableRow>();

            if (_defaultData.ContainsKey(entityName))
            {
                for(int i = 0; i < _defaultData[entityName].Length; i++)
                {
                    result.AddRow(_defaultData[entityName][i].Name, _defaultData[entityName][i].Value);
                    rows.Add(_defaultData[entityName][i].Name, result.Rows[i]);
                }
            }

            foreach(var row in customFields.Rows)
            {
                if(rows.TryGetValue(row[Constants.SpecFlow.TABLE_KEY], out TableRow rowToUpdate))
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

            if (_defaultData.ContainsKey(entityName))
            {
                foreach (DefaultDataField defaultDataField in _defaultData[entityName])
                {
                    toCreate[defaultDataField.Name] = ObjectConverter.ToCrmObject(entityName, defaultDataField.Name, defaultDataField.Value, _crmContext);
                }
            }

            foreach (TableRow row in customFields.Rows)
            {
                toCreate[row[Constants.SpecFlow.TABLE_KEY]] = ObjectConverter.ToCrmObject(entityName, row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE], _crmContext);
            }
            return toCreate;
        }
    }




}
