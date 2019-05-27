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
        public CrmService Service { get; set; }
        public MetadataCache Metadata { get; set; }
        public RecordBuilder RecordBuilder { get; set; }
        public CrmConnectionString ConnectionInfo { get; set; }
        public AliasedRecordCache RecordCache { get; set; }
        public TableConverter TableConverter { get; set; }

        public CommandProcessor CommandProcessor { get; set; }

        public int LanguageCode { get; set; }

        public CrmTestingContext()
        {
            ConnectionInfo = CrmConnectionString.CreateFromAppConfig();
            Service = new CrmService(ConnectionInfo);
            Metadata = new MetadataCache(Service);
            RecordCache = new AliasedRecordCache(Service, Metadata);
            RecordBuilder = new RecordBuilder(this);
            TableConverter = new TableConverter(this);
            LanguageCode = GetLanguageCode();
            CommandProcessor = new CommandProcessor();

            Service.RecordCache = RecordCache;
        }

        private int GetLanguageCode()
        {
            if (!int.TryParse(HelperMethods.GetAppSettingsValue("LanguageCode"), out int lcid))
                throw new InvalidCastException(string.Format("AppSettings languagecode must be an integer i.e. 1033 for english"));

            return lcid;
        }

        
    }
}
