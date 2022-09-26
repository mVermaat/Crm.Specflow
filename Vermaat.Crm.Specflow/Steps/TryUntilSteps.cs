using Azure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow.Steps
{
    public class TryUntilSteps
    {
        private readonly CrmTestingContext _crmContext;
        private readonly SeleniumTestingContext _seleniumContext;

        public TryUntilSteps(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        [Then(@"within ([0-9]+) seconds ([^\s]+) has the following values")]
        public void ThenAliasHasValues(int seconds, string alias, Table criteria)
        {
            EntityReference aliasRef = _crmContext.RecordCache[alias];
            _crmContext.TableConverter.ConvertTable(aliasRef.LogicalName, criteria);

            TryUntil(new AssertCrmRecordCommand(_crmContext, aliasRef, criteria), seconds);
        }

        [Then(@"within ([0-9]+) seconds a ([^\s]+) exists with the following values")]
        [Then(@"within ([0-9]+) seconds an ([^\s]+) exists with the following values")]
        public Entity ThenRecordExists(int seconds, string entityName, Table criteria)
        {
            return ThenRecordCountExists(seconds, 1, entityName, criteria)[0];
        }

        [Then(@"within ([0-9]+) seconds([0-9]+) ([^\s]+) records exist with the following values")]
        public DataCollection<Entity> ThenRecordCountExists(int seconds, int amount, string entityName, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);

            return TryUntil<GetRecordsCommand, DataCollection<Entity>>(new GetRecordsCommand(_crmContext, entityName, criteria), seconds, 
                r => r.Count == amount,
                r => $"When looking for records for {entityName}, expected {amount}, but found {r.Count} records");
        }

        private void TryUntil<TCommand>(TCommand command, int seconds) where TCommand : ICommand
        {
            _crmContext.CommandProcessor.Execute(new TryUntilCommand<TCommand>(_crmContext, command, TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(5)));
        }

        private TResult TryUntil<TCommand, TResult>(TCommand command, int seconds, Func<TResult, bool> assertFunc, Func<TResult,string> timeoutMessageFunc) where TCommand : ICommandFunc<TResult>
        {
            return _crmContext.CommandProcessor.Execute(new TryUntilCommandFunc<TCommand, TResult>(_crmContext, command, 
                TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(5), assertFunc, timeoutMessageFunc));
        }
    }
}
