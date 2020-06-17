using System;
using System.Collections.Generic;
using System.Text;

namespace PowerPlatform.SpecflowExtensions.Interfaces
{
    public interface ICrmContext
    {
        int LanguageCode { get; }
        AliasedRecordCache RecordCache { get; }
        TableConverter TableConverter { get; }        
    }
}
