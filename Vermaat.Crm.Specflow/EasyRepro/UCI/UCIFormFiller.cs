using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;


namespace Vermaat.Crm.Specflow.EasyRepro.UCI
{
    class UCIFormFiller : IFormFiller
    {
        private readonly Microsoft.Dynamics365.UIAutomation.Api.UCI.Entity _entity;

        public UCIFormFiller(Microsoft.Dynamics365.UIAutomation.Api.UCI.Entity entity)
        {
            _entity = entity;
        }

        public void SetCompositeField(string parentField, IEnumerable<(string fieldName, string fieldValue)> fields)
        {
            foreach (var (fieldName, fieldValue) in fields)
            {
                SetTextField(fieldName, fieldValue);
            }
        }

        public void SetDateTimeField(string fieldName, DateTime fieldValue)
        {
            _entity.SetValue(fieldName, fieldValue);
        }

        public void SetLookupValue(string fieldName, EntityReference value)
        {
            throw new NotImplementedException();
        }

        public void SetOptionSetField(string fieldName, string fieldValue)
        {
            _entity.SetValue(new OptionSet { Name = fieldName, Value = fieldValue });
        }

        public void SetTextField(string fieldName, string fieldValue)
        {
            _entity.SetValue(fieldName, fieldValue);
        }

        public void SetTwoOptionField(string fieldName, bool fieldValue)
        {
            _entity.SetValue(new BooleanItem { Name = fieldName, Value = fieldValue });
        }
    }
}
