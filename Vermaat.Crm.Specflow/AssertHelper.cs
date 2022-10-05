﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow
{
    /// <summary>
    /// Helper class to help asserting for CRM Specific objects
    /// </summary>
    public static class AssertHelper
    {
        /// <summary>
        /// Special Assert.AreEqual which can assert EntityReference, OptionSet and Money object. For all other types, the default Assert is called.
        /// </summary>
        /// <param name="actualValue"></param>
        /// <param name="expectedValue"></param>
        public static void AreEqual(object actualValue, object expectedValue, string attributeName)
        {
            if (actualValue == null && expectedValue == null)
                return;

            var type = expectedValue != null ? expectedValue.GetType() : actualValue.GetType();

            if (type == typeof(EntityReference))
                Assert.AreEqual(((EntityReference)expectedValue)?.Id, ((EntityReference)actualValue)?.Id, $"Field {attributeName} is different");
            else if (type == typeof(OptionSetValue))
                Assert.AreEqual(((OptionSetValue)expectedValue)?.Value, ((OptionSetValue)actualValue)?.Value, $"Field {attributeName} is different");
            else if (type == typeof(Money))
                Assert.AreEqual(((Money)expectedValue)?.Value, ((Money)actualValue)?.Value, $"Field {attributeName} is different");
            else if(type == typeof(OptionSetValueCollection))
            {
                var expected = ((OptionSetValueCollection)expectedValue).Select(e => e.Value).ToArray();
                var actual = ((OptionSetValueCollection)actualValue).Select(a => a.Value).ToArray();
                Assert.AreEqual(expected.Length, actual.Length, $"Expected Values: {string.Join(", ", expected)} | Actual Values: {string.Join(", ", actual)}");
                Assert.AreEqual(expected.Except(actual).Count(), 0, $"Expected Values: {string.Join(", ", expected)} | Actual Values: {string.Join(", ", actual)}");
                Assert.AreEqual(actual.Except(expected).Count(), 0, $"Expected Values: {string.Join(", ", expected)} | Actual Values: {string.Join(", ", actual)}");
            }
            else if(type == typeof(EntityCollection))
            {
                var expected = ParseEntityCollection((EntityCollection)expectedValue);
                var actual = ParseEntityCollection((EntityCollection)actualValue);

                Assert.AreEqual(expected.Length, actual.Length, $"Expected Values: {string.Join(", ", expected)} | Actual Values: {string.Join(", ", actual)}");
                Assert.AreEqual(expected.Except(actual).Count(), 0, $"Expected Values: {string.Join(", ", expected)} | Actual Values: {string.Join(", ", actual)}");
                Assert.AreEqual(actual.Except(expected).Count(), 0, $"Expected Values: {string.Join(", ", expected)} | Actual Values: {string.Join(", ", actual)}");
            }
            else
                Assert.AreEqual(expectedValue, actualValue, $"Field {attributeName} is different");
        }

        private static Guid[] ParseEntityCollection(EntityCollection col)
        {
            if (col == null || col.Entities == null || col.Entities.Count == 0)
                return new Guid[0];

            string entityName = col.EntityName ?? col.Entities[0]?.LogicalName;
            if(!string.IsNullOrEmpty(entityName) && entityName.Equals("activityparty"))
            {
                return col.Entities.Select(e => e.GetAttributeValue<EntityReference>("partyid").Id).ToArray();
            }
            else
            {
                return col.Entities.Select(e => e.Id).ToArray();
            } 
                
        }

        /// <summary>
        /// Asserts if an entity has all of the field/value combinations.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        public static void HasProperties(Entity record, Table criteria, CrmTestingContext context)
        {
            List<string> errors = new List<string>();
            foreach (var row in criteria.Rows)
            {
                var actualValue = record.Contains(row[Constants.SpecFlow.TABLE_KEY]) ? record[row[Constants.SpecFlow.TABLE_KEY]] : null;
                var expectedValue = ObjectConverter.ToCrmObject(record.LogicalName, row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE], context);

                try
                {
                    AreEqual(actualValue, expectedValue, row[Constants.SpecFlow.TABLE_KEY]);
                }
                catch(AssertFailedException ex)
                {
                    Logger.WriteLine(ex.Message);
                    errors.Add(ex.Message);
                }
            }

            if(errors.Count > 0)
            {
                Assert.Fail($"At least one error occured when asseting fields. Errors: {string.Join(Environment.NewLine, errors)}");
            }
        }
    }
}
