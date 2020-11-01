using BoDi;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class AssertCrmRecordCommand : ApiOnlyCommand
    {
        private readonly EntityReference _crmRecord;
        private readonly Table _criteria;

        public AssertCrmRecordCommand(IObjectContainer container, EntityReference crmRecord, Table criteria) : base(container)
        {
            _crmRecord = crmRecord;
            _criteria = criteria;
        }

        public override void Execute()
        {
            ColumnSet columns = new ColumnSet(_criteria.Rows.Select(r => r[Constants.SpecFlow.TABLE_KEY]).ToArray());
            Entity record = GlobalContext.ConnectionManager.CurrentCrmService.Retrieve(_crmRecord, columns);
            HasProperties(record);
        }

        private void HasProperties(Entity record)
        {
            List<string> errors = new List<string>();
            foreach (var row in _criteria.Rows)
            {
                var actualValue = record.Contains(row[Constants.SpecFlow.TABLE_KEY]) ? record[row[Constants.SpecFlow.TABLE_KEY]] : null;
                var expectedValue = ObjectConverter.ToCrmObject(record.LogicalName, row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE], _crmContext);

                if(AreEqual(actualValue, expectedValue, row[Constants.SpecFlow.TABLE_KEY], out string message))
                {
                    Logger.WriteLine($"Values for {row[Constants.SpecFlow.TABLE_KEY]} match. {message}");
                }
                else
                {
                    errors.Add($"Values for {row[Constants.SpecFlow.TABLE_KEY]} don't match. {message}");
                    Logger.WriteLine(errors.Last());
                }
            }

            if (errors.Count > 0)
            {
                throw new TestExecutionException(Constants.ErrorCodes.RECORD_DOESNT_MATCH, string.Join(Environment.NewLine, errors));
            }
        }

        private bool AreEqual(object actualValue, object expectedValue, string attributeName, out string values)
        {
            if (actualValue == null && expectedValue == null)
            {
                values = "Both values are null";
                return true;
            }

            var type = expectedValue != null ? expectedValue.GetType() : actualValue.GetType();

            object actualPrimitive = actualValue, expectedPrimitive = expectedValue;
            
            if (type == typeof(EntityReference))
            {
                expectedPrimitive = ((EntityReference)expectedValue)?.Id;
                actualPrimitive = ((EntityReference)actualValue)?.Id;
            }
            else if (type == typeof(OptionSetValue))
            {
                expectedPrimitive = ((OptionSetValue)expectedValue)?.Value;
                actualPrimitive = ((OptionSetValue)actualValue)?.Value;
            }
            else if (type == typeof(Money))
            {
                expectedPrimitive = ((Money)expectedValue)?.Value;
                actualPrimitive = ((Money)actualValue)?.Value;
            }

            values = $"Expected: {expectedPrimitive}. Actual: {actualPrimitive}";
            return expectedPrimitive.Equals(actualPrimitive);
        }
    }
}
