using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public interface IBrowserEntity
    {
        void DeleteRecord();
        void FillForm(CrmTestingContext crmContext, string entityName, Table dataTable);
        string GetEntityName();
        Guid GetId();
        bool IsFieldOnForm(string fieldLogicalName);
        bool IsFieldVisible(string fieldLogicalName);
        void SaveRecord(bool saveIfDuplicate);

        //void SetValue

    }
}
