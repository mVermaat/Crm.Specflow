using FluentAssertions;
using Microsoft.Xrm.Sdk;
using PowerPlatform.SpecflowExtensions.Commands;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Steps
{
    [Binding]
    public class CrudSteps
    {
        private readonly ICrmContext _crmContext;
        private readonly ISeleniumContext _seleniumContext;

        public CrudSteps(ICrmContext crmContext, ISeleniumContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        [Given(@"an existing ([^\s]+) named (.*) with the following values")]
        public void GivenExistingWithValues(string entityName, string alias, Table criteria)
        {
            Entity entity = ThenRecordExists(entityName, criteria);
            _crmContext.RecordCache.Add(alias, entity, false);
        }

        [Given(@"a ([^\s]+) named (.*) with the following values")]
        [Given(@"an ([^\s]+) named (.*) with the following values")]
        [When(@"a ([^\s]+) named (.*) is created with the following values")]
        [When(@"an ([^\s]+) named (.*) is created with the following values")]
        public void GivenEntityWithValues(string entityName, string alias, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);
            _crmContext.CommandProcessor.Execute(new CreateRecordCommand(_crmContext, _seleniumContext, entityName, criteria, alias));
        }

        [Given(@"a related ([^\s]+) from (.*) named (.*) with the following values")]
        [When(@"a related ([^\s]+) from (.*) named (.*) is created with the following values")]
        public void GivenRelatedEntityWithValues(string entityName, string parentAlias, string childAlias, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);
            _crmContext.CommandProcessor.Execute(new CreateRelatedRecordCommand(_crmContext, _seleniumContext, entityName, criteria, childAlias, parentAlias));
        }

        [When(@"(.*) is updated with the following values")]
        public void WhenAliasIsUpdated(string alias, Table criteria)
        {
            EntityReference aliasRef = _crmContext.RecordCache[alias];
            _crmContext.TableConverter.ConvertTable(aliasRef.LogicalName, criteria);
            _crmContext.CommandProcessor.Execute(new UpdateRecordCommand(_crmContext, _seleniumContext, aliasRef, criteria));
        }


        [Then(@"(.*) has the following values")]
        public void ThenAliasHasValues(string alias, Table criteria)
        {
            EntityReference aliasRef = _crmContext.RecordCache[alias];
            _crmContext.TableConverter.ConvertTable(aliasRef.LogicalName, criteria);

            _crmContext.CommandProcessor.Execute(new AssertCrmRecordCommand(_crmContext, aliasRef, criteria));
        }

        [Then(@"a ([^\s]+) exists with the following values")]
        [Then(@"an ([^\s]+) exists with the following values")]
        public Entity ThenRecordExists(string entityName, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);
            DataCollection<Entity> records = _crmContext.CommandProcessor.Execute(new GetRecordsCommand(_crmContext, entityName, criteria));
            records.Should().HaveCount(1, string.Format("When looking for records for {0}, expected 1, but found {1} records", entityName, records.Count));

            return records[0];
        }

        [Then(@"a ([^\s]+) named (.*) exists with the following values")]
        [Then(@"an ([^\s]+) named (.*) exists with the following values")]
        public Entity ThenRecordExistsAndGiveAlias(string entityName, string alias, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);
            DataCollection<Entity> records = _crmContext.CommandProcessor.Execute(new GetRecordsCommand(_crmContext, entityName, criteria));
            records.Should().HaveCount(1, string.Format("When looking for records for {0}, expected 1, but found {1} records", entityName, records.Count));

            _crmContext.RecordCache.Add(alias, records[0], false);

            return records[0];
        }

        [Given(@"that (.*)'s (.*) is named (.*)")]
        [Then(@"(.*)'s (.*) is named (.*)")]
        public void ThenAliasFieldIsAliased(string alias, string lookupField, string lookupAlias)
        {
            _crmContext.CommandProcessor.Execute(new SetLookupAsAliasCommand(_crmContext, alias, lookupField, lookupAlias));
        }

    }
}
