using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    interface IFormFiller
    {
        void SetLookupValue(string fieldName, EntityReference value);
        void SetOptionSetField(string fieldName, string fieldValue);
        void SetTextField(string fieldName, string fieldValue);
        void SetTwoOptionField(string fieldName, bool fieldValue);
        void SetDateTimeField(string fieldName, DateTime fieldValue);
        void SetCompositeField(string parentField, IEnumerable<(string fieldName, string fieldValue)> fields);
    }
}
