using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    class OpportunityCloseDialogField : Field
    {
        protected override string LogicalName => GetLogicalName();

        private string GetLogicalName()
        {
            if (Metadata.LogicalName == "opportunitystatuscode")
                return "statusreason_id";
            else
                return base.LogicalName + "_id";
        }

        public OpportunityCloseDialogField(UCIApp app, AttributeMetadata metadata) 
            : base(app, metadata)
        {
        }

        protected override void SetLookupValue(EntityReference fieldValue, string fieldValueText)
        {
            App.App.Entity.SetValue(new LookupItem { Name = LogicalName, Value = fieldValueText });
        }
    }
}
