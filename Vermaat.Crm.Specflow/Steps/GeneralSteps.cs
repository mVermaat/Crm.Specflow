using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow;

namespace Vermaat.Crm.Specflow.Steps
{
    [Binding]
    [DeploymentItem("DefaultData.xml")]
    public class GeneralSteps
    {
        private CrmTestingContext _crmContext;
        private StepProcessor _stepProcessor;

        public GeneralSteps(CrmTestingContext crmContext, StepProcessor stepProcessor)
        {
            _crmContext = crmContext;
            _stepProcessor = stepProcessor;
        }


        #region Given

        [Given(@"an existing ([^\s]+) named (.*) with the following values")]
        public void GivenExistingWithValues(string entityName, string alias, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);
            var entity = ThenRecordExists(entityName, criteria);
            _crmContext.RecordCache.Add(alias, entity);
        }

        [Given(@"a ([^\s]+) named (.*) with the following values")]
        [Given(@"an ([^\s]+) named (.*) with the following values")]
        [When(@"a ([^\s]+) named (.*) is created with the following values")]
        [When(@"an ([^\s]+) named (.*) is created with the following values")]
        public void GivenEntityWithValues(string entityName, string alias, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);
            _stepProcessor.GeneralCrm.CreateAliasedRecord(entityName, criteria, alias);
        }

        [Given(@"(.*) has the process stage (.*)")]
        [When(@"the process stage of (.*) is changed to (.*)")]
        public void SetProcessStage(string alias, string stageName)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _stepProcessor.BusinessProcessFlow.MoveToStage(aliasRef, stageName);
        }


        #endregion

        #region When

        [When(@"(.*) is moved to the next process stage")]
        public void MoveToNextStage(string alias)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _stepProcessor.BusinessProcessFlow.MoveNext(aliasRef);
        }

        [When(@"(.*) is updated with the following values")]
        public void WhenAliasIsUpdated(string alias, Table criteria)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _crmContext.TableConverter.ConvertTable(aliasRef.LogicalName, criteria);
            _stepProcessor.GeneralCrm.UpdateRecord(aliasRef, criteria);
        }

        [When(@"all asynchronous processes for (.*) are finished")]
        public void WhenAsyncJobsFinished(string alias)
        {
            var aliasRef = _crmContext.RecordCache[alias];

            int tryCount = 0;
            while (tryCount < 15 && QueryHelper.HasOpenSystemJobs(aliasRef.Id, _crmContext.Service))
            {
                Thread.Sleep(2000);
                tryCount++;
            }

            Assert.AreNotEqual(15, tryCount, "Not all system jobs were finished on time");
        }

        [When(@"the status of (.*) is changed to (.*)")]
        public void WhenStatusChanges(string alias, string newStatus)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _stepProcessor.GeneralCrm.UpdateStatus(aliasRef, newStatus);
        }

        [When(@"(.*) is deleted")]
        public void WhenAliasIsDeleted(string alias)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _stepProcessor.GeneralCrm.DeleteRecord(aliasRef);
            _crmContext.RecordCache.Remove(alias);
        }

        [When(@"(.*) is assigned to (.*)")]
        public void WhenAliasIsAssignedToAlias(string aliasToAssign, string aliasToAssignTo)
        {
            _stepProcessor.GeneralCrm.AssignRecord(_crmContext.RecordCache[aliasToAssignTo], _crmContext.RecordCache[aliasToAssign]);
        }

        #endregion

        #region Then


        [Then(@"the process stage of (.*) is (.*)")]
        public void AssertProcessStage(string alias, string stageName)
        {
            var aliasRef = _crmContext.RecordCache[alias];

            var actualStage = _stepProcessor.BusinessProcessFlow.GetCurrentStage(aliasRef);
            var expectedStage = _stepProcessor.BusinessProcessFlow.GetStageByName(actualStage.ProcessId, stageName);

            Assert.AreEqual(expectedStage, actualStage.StageId);
        }

        [Then(@"(.*) has the following values")]
        public void ThenAliasHasValues(string alias, Table criteria)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _crmContext.TableConverter.ConvertTable(aliasRef.LogicalName, criteria);

            var columns = new ColumnSet(criteria.Rows.Select(r => r["Property"]).ToArray());
            var record = _crmContext.Service.Retrieve(aliasRef, columns);

            AssertHelper.HasProperties(record, criteria, _crmContext);
        }

        [Then(@"a (.*) exists with the following values")]
        [Then(@"an (.*) exists with the following values")]
        public Entity ThenRecordExists(string entityName, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);

            var query = QueryHelper.CreateQueryExpressionFromTable(entityName, criteria, _crmContext);
            var records = HelperMethods.ExecuteWithRetry(20, 500, () => _crmContext.Service.RetrieveMultiple(query));

            Assert.AreEqual(1, records.Entities.Count, string.Format("When looking for records for {0}, expected 1, but found {1} records", entityName, records.Entities.Count));

            return records.Entities[0];
        }


        #endregion


    }
}
