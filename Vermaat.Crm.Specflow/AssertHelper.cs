using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Entities;

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
            else if (type == typeof(OptionSetValueCollection))
            {
                var expected = ((OptionSetValueCollection)expectedValue).Select(e => e.Value).ToArray();
                var actual = ((OptionSetValueCollection)actualValue).Select(a => a.Value).ToArray();
                Assert.AreEqual(expected.Length, actual.Length, $"Expected Values: {string.Join(", ", expected)} | Actual Values: {string.Join(", ", actual)}");
                Assert.AreEqual(expected.Except(actual).Count(), 0, $"Expected Values: {string.Join(", ", expected)} | Actual Values: {string.Join(", ", actual)}");
                Assert.AreEqual(actual.Except(expected).Count(), 0, $"Expected Values: {string.Join(", ", expected)} | Actual Values: {string.Join(", ", actual)}");
            }
            else if (type == typeof(EntityCollection))
            {
                var expected = Party.FromEntityCollection((EntityCollection)expectedValue);
                var actual = Party.FromEntityCollection((EntityCollection)actualValue);

                MatchesParties(attributeName, expected, actual);
            }
            else
                Assert.AreEqual(expectedValue, actualValue, $"Field {attributeName} is different");
        }

        private static void MatchesParties(string attributeName, Party[] expected, Party[] actual)
        {
            var actualList = actual.ToList();
            List<string> errors = new List<string>();
            foreach (var expectedParty in expected)
            {
                Party actualMatch = null;
                if (!string.IsNullOrWhiteSpace(expectedParty.EmailAddress))
                    actualMatch = actualList.FirstOrDefault(a => expectedParty.EmailAddress.Equals(a.EmailAddress));
                else if (expectedParty.ConnectedParty != null && expectedParty.ConnectedParty.Id != Guid.Empty)
                    actualMatch = actualList.FirstOrDefault(a => expectedParty.ConnectedParty.Id.Equals(a.ConnectedParty?.Id));


                if (actualMatch != null)
                    actualList.Remove(actualMatch);
                else
                    errors.Add($"Expected value missing: " + expectedParty.ToString());
            }

            foreach (var extraItem in actualList)
            {
                errors.Add($"Extra value: " + extraItem.ToString());
            }

            Assert.AreEqual(0, errors.Count, $"Field {attributeName} is different. {string.Join(",", errors)}");
        }

        public static void MatchesRegex(string regex, object expectedValue, string attributeName)
        {
            Assert.IsNotNull(expectedValue, $"Expected {attributeName} to have data, but it's empty");

            var type = expectedValue.GetType();
            if (type != typeof(string))
                Assert.Fail("Can only do regex comparisons for text fields.");

            Assert.IsTrue(Regex.IsMatch((string)expectedValue, regex), $"Expected {attributeName} to match regex {regex}. {expectedValue} doesn't match it");
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
                var expectedValue = row[Constants.SpecFlow.TABLE_VALUE];

                var condition = criteria.ContainsColumn(Constants.SpecFlow.TABLE_CONDITION)
                    ? (row[Constants.SpecFlow.TABLE_CONDITION] ?? "Equal")
                    : "Equal";

                try
                {
                    switch (condition.ToLowerInvariant())
                    {
                        case "equal": AreEqual(actualValue, ObjectConverter.ToCrmObject(record.LogicalName, row[Constants.SpecFlow.TABLE_KEY], expectedValue, context), row[Constants.SpecFlow.TABLE_KEY]); break;
                        case "notnull": Assert.IsNotNull(actualValue, $"Expected {row[Constants.SpecFlow.TABLE_KEY]} to have data, but it's empty"); break;
                        case "regex": MatchesRegex(expectedValue, actualValue, row[Constants.SpecFlow.TABLE_KEY]); break;
                    }


                }
                catch (AssertFailedException ex)
                {
                    Logger.WriteLine(ex.Message);
                    errors.Add(ex.Message);
                }
            }

            if (errors.Count > 0)
            {
                Assert.Fail($"At least one error occured when asseting fields. Errors: {string.Join(Environment.NewLine, errors)}");
            }
        }


    }
}
