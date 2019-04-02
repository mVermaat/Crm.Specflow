using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public interface IBrowser : IDisposable
    {
        void Login(CrmConnectionString connectionString);
        void OpenRecord(EntityReference crmRecord);
        void OpenRecord(string logicalName, Guid? id = null);

        IBrowserEntity Entity { get; }
    }
}
