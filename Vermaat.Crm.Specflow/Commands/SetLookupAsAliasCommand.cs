using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Vermaat.Crm.Specflow.Commands
{
    public class SetLookupAsAliasCommand : ApiOnlyCommand
    {
        private readonly string _alias;
        private readonly string _lookupFieldName;
        private readonly string _lookupAlias;

        public SetLookupAsAliasCommand(CrmTestingContext crmContext, string alias, string lookupFieldName, string lookupAlias) : base(crmContext)
        {
            _alias = alias;
            _lookupFieldName = lookupFieldName;
            _lookupAlias = lookupAlias;
        }

        public override void Execute()
        {
            var aliasRef = _crmContext.RecordCache[_alias];
            var attribute = GlobalTestingContext.Metadata.GetAttributeMetadata(aliasRef.LogicalName, _lookupFieldName, _crmContext.LanguageCode);
            Entity record = GlobalTestingContext.ConnectionManager.CurrentConnection.Retrieve(aliasRef, new ColumnSet(attribute.LogicalName));
            var value = record.Contains(attribute.LogicalName) ? record[attribute.LogicalName] : null;

            if(value == null )
            {
                throw new TestExecutionException(Constants.ErrorCodes.VALUE_NULL, attribute.LogicalName);
            }
            else if(value.GetType() != typeof(EntityReference))
            {
                throw new TestExecutionException(Constants.ErrorCodes.INVALID_DATATYPE, attribute.LogicalName, "lookup");
            }
            _crmContext.RecordCache.Add(_lookupAlias, (EntityReference)value, false);
        }
    }
}
