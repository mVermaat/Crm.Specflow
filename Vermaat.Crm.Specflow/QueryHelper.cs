using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow
{
    static class QueryHelper
    {
        public static bool HasOpenSystemJobs(Guid regardingId)
        {
            QueryExpression qe = new QueryExpression("asyncoperation")
            {
                ColumnSet = new ColumnSet(false),
                TopCount = 1
            };
            qe.Criteria.AddCondition("regardingobjectid", ConditionOperator.Equal, regardingId);
            qe.Criteria.AddCondition("statuscode", ConditionOperator.NotIn, new object[] { 10, 30, 31, 32 });

            return GlobalTestingContext.ConnectionManager.AdminConnection.RetrieveMultiple(qe).Entities.Count > 0;
        }

        public static QueryExpression CreateQueryExpressionFromTable(string entityName, Table criteria, CrmTestingContext context)
        {
            Logger.WriteLine($"Creating Query for {entityName}");
            QueryExpression qe = new QueryExpression(entityName)
            {
                ColumnSet = new ColumnSet()
            };

            bool hasConditionColumn = criteria.ContainsColumn(Constants.SpecFlow.TABLE_CONDITION);

            foreach (var row in criteria.Rows)
            {
                var crmValue = ObjectConverter.ToCrmObject(entityName, row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE], context, ConvertedObjectType.Primitive);
                qe.Criteria.AddCondition(CreateConditionExpression(row, crmValue, hasConditionColumn));
            }

            return qe;
        }

        private static ConditionExpression CreateConditionExpression(TableRow row, object crmValue, bool hasConditionColumn)
        {
            var expression = new ConditionExpression();

            expression.AttributeName = row[Constants.SpecFlow.TABLE_KEY];
            expression.Operator = GetOperator(row, hasConditionColumn, crmValue);
            if(crmValue != null)
                expression.Values.Add(crmValue);

            Logger.WriteLine($"Adding condition {expression.AttributeName} {expression.Operator} {(crmValue != null ? HelperMethods.CrmObjectToPrimitive(crmValue) : "")}");

            return expression;
        }

        private static ConditionOperator GetOperator(TableRow row, bool hasConditionColumn, object crmValue)
        {
            if (hasConditionColumn && !string.IsNullOrEmpty(row[Constants.SpecFlow.TABLE_CONDITION]))
                try {
                    return (ConditionOperator)Enum.Parse(typeof(ConditionOperator), row[Constants.SpecFlow.TABLE_CONDITION]);
                }
                catch(ArgumentException ex)
                {
                    throw new TestExecutionException(Constants.ErrorCodes.FAILED_TO_PARSE_CONDITION_OPERATOR, ex,
                        row[Constants.SpecFlow.TABLE_CONDITION], ex.Message);
                }
            else if (crmValue == null)
                return ConditionOperator.Null;
            else
                return ConditionOperator.Equal;
        }
    }
}
