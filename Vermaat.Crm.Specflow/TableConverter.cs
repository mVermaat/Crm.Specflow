using System;
using Reqnroll;

namespace Vermaat.Crm.Specflow
{
    public class TableConverter
    {
        private readonly CrmTestingContext _context;

        public event EventHandler<TableEventArgs> OnTableProcessing;
        public event EventHandler<TableEventArgs> OnTableProcessed;

        public event EventHandler<DataTableRowEventArgs> OnRowProcessing;
        public event EventHandler<DataTableRowEventArgs> OnRowProcessed;

        public TableConverter(CrmTestingContext context)
        {
            _context = context;
        }

        public void ConvertTable(string entityName, Table table)
        {
            OnTableProcessing?.Invoke(this, new TableEventArgs(entityName, table));

            foreach (var row in table.Rows)
            {
                OnRowProcessing?.Invoke(this, new DataTableRowEventArgs(entityName, row));

                var attribute = GlobalTestingContext.Metadata.GetAttributeMetadata(entityName, row[Constants.SpecFlow.TABLE_KEY], GlobalTestingContext.LanguageCode);

                if (row.ContainsKey(Constants.SpecFlow.TABLE_KEY))
                    row[Constants.SpecFlow.TABLE_KEY] = attribute.LogicalName;

                OnRowProcessed?.Invoke(this, new DataTableRowEventArgs(entityName, row));
            }

            OnTableProcessed?.Invoke(this, new TableEventArgs(entityName, table));
        }

        public void LocalizeColumn(Table table, string column, int languageCode)
        {
            foreach (var row in table.Rows)
            {
                if (row.ContainsKey(column) && !string.IsNullOrWhiteSpace(row[column]))
                {
                    var original = row[column];
                    row[column] = GlobalTestingContext.LocalizedTexts[row[column], languageCode] ?? row[column];
                    Logger.WriteLine($"Localized {original} to {row[column]}");
                }
            }
        }

        public class DataTableRowEventArgs : EventArgs
        {
            public DataTableRowEventArgs(string entityName, DataTableRow row)
            {
                Row = row;
                EntityName = entityName;
            }

            public DataTableRow Row { get; private set; }
            public string EntityName { get; private set; }
        }

        public class TableEventArgs : EventArgs
        {
            public TableEventArgs(string entityName, Table table)
            {
                Table = table;
                EntityName = entityName;
            }

            public Table Table { get; private set; }
            public string EntityName { get; private set; }
        }
    }
}
