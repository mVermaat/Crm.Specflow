using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow
{
    [Binding]
    public class CrmTestingContext
    {

        public RecordBuilder RecordBuilder { get; }
        public TableConverter TableConverter { get; }

        public CommandProcessor CommandProcessor { get; set; }

        public AliasedRecordCache RecordCache { get; }

        public char Delimiter { get; }

        private readonly string[] _targets;
        

        public CrmTestingContext(ScenarioContext scenarioContext)
        {
            RecordBuilder = new RecordBuilder(this);
            TableConverter = new TableConverter(this);
            CommandProcessor = new CommandProcessor(scenarioContext);
            RecordCache = new AliasedRecordCache(GlobalTestingContext.Metadata);

            _targets = ConfigurationManager.AppSettings["Target"]
                .ToLower()
                .Split('_')
                .Select(splitted => splitted.Trim())
                .ToArray();
            Delimiter = GetDelimiter(scenarioContext);
        }

        private char GetDelimiter(ScenarioContext scenarioContext)
        {
            var delimiterTag = scenarioContext.ScenarioInfo.Tags.FirstOrDefault(t => t.StartsWith("Delimiter:"));

            if(delimiterTag != null)
            {
                if (delimiterTag.Length != 11)
                {
                    Logger.WriteLine("Unexpected length for custom delimiter. It must be exactly 1 character after the 'Delimiter:'");
                }
                else
                {
                    Logger.WriteLine($"Using custom delimiter: {delimiterTag[delimiterTag.Length - 1]}");
                    return delimiterTag[delimiterTag.Length - 1];
                }
            }

            Logger.WriteLine("Using default delimiter: ,");
            return ',';            
        }

        public bool IsTarget(string target)
        {
            if (string.IsNullOrWhiteSpace(target))
                return false;

            return _targets.Contains(target.ToLower());
        }
        
    }
}
