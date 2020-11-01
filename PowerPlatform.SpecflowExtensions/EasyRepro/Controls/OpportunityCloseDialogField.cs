using Microsoft.Xrm.Sdk.Metadata;
using PowerPlatform.SpecflowExtensions.EasyRepro.FieldTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Controls
{
    internal class OpportunityCloseDialogField : Field
    {
        public OpportunityCloseDialogField(AttributeMetadata metadata, string control) 
            : base(metadata, control)
        {
        }

        protected override void SetDateTimeField(DateTimeValue value)
        {
            throw new NotImplementedException();
        }

        protected override void SetDecimalField(DecimalValue value)
        {
            throw new NotImplementedException();
        }

        protected override void SetDoubleField(DoubleValue value)
        {
            throw new NotImplementedException();
        }

        protected override void SetIntegerField(IntegerValue value)
        {
            throw new NotImplementedException();
        }

        protected override void SetLongField(LongValue value)
        {
            throw new NotImplementedException();
        }

        protected override void SetLookupValue(LookupValue value)
        {
            throw new NotImplementedException();
        }

        protected override void SetMoneyField(DecimalValue value)
        {
            throw new NotImplementedException();
        }

        protected override void SetMultiSelectOptionSetField(MultiSelectOptionSetValue value)
        {
            throw new NotImplementedException();
        }

        protected override void SetOptionSetField(OptionSetValue value)
        {
            throw new NotImplementedException();
        }

        protected override void SetTextField(string fieldValue)
        {
            throw new NotImplementedException();
        }

        protected override void SetTwoOptionField(BooleanValue value)
        {
            throw new NotImplementedException();
        }
    }
}
