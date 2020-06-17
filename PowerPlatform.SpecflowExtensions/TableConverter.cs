using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions
{
    public class TableConverter
    {
        private readonly ICrmContext _context;

        public event EventHandler<TableEventArgs> OnTableProcessing;
        public event EventHandler<TableEventArgs> OnTableProcessed;

        public event EventHandler<TableRowEventArgs> OnRowProcessing;
        public event EventHandler<TableRowEventArgs> OnRowProcessed;

        public TableConverter(ICrmContext context)
        {
            _context = context;
        }

        public void ConvertTable(string entityName, Table table)
        {
            OnTableProcessing?.Invoke(this, new TableEventArgs(entityName, table));

            foreach (var row in table.Rows)
            {
                OnRowProcessing?.Invoke(this, new TableRowEventArgs(entityName, row));

                var attribute = GlobalContext.Metadata.GetAttributeMetadata(entityName, row[Constants.SpecFlow.TABLE_KEY], _context.LanguageCode);

                if (row.ContainsKey(Constants.SpecFlow.TABLE_KEY))
                    row[Constants.SpecFlow.TABLE_KEY] = attribute.LogicalName;

                OnRowProcessed?.Invoke(this, new TableRowEventArgs(entityName, row));
            }

            OnTableProcessed?.Invoke(this, new TableEventArgs(entityName, table));
        }

        public class TableRowEventArgs : EventArgs
        {
            public TableRowEventArgs(string entityName, TableRow row)
            {
                Row = row;
                EntityName = entityName;
            }

            public TableRow Row { get; private set; }
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
