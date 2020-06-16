using System;
using System.Collections.Generic;
using System.Text;

namespace PowerPlatform.SpecflowExtensions.Interfaces
{
    public interface ICrmContext
    {
        AliasedRecordCache RecordCache { get; }
    }
}
