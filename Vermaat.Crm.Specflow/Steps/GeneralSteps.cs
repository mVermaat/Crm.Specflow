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

        public GeneralSteps(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
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
        public void GivenEntityWithValues(string entityName, string alias, Table criteria)
        {
            _crmContext.TableConverter.ConvertTable(entityName, criteria);

            Entity toCreate = _crmContext.RecordBuilder.SetupEntityWithDefaults(entityName, criteria);

            _crmContext.Service.Create(toCreate, alias);
        }

        [Given(@"(.*) has the process stage (.*)")]
        [When(@"the process stage of (.*) is changed to (.*)")]
        public void SetProcessStage(string alias, string stageName)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            BusinessProcessHelper.MoveToStage(aliasRef, stageName, _crmContext.Service);
        }


        #endregion

        #region When

        [When(@"(.*) is moved to the next process stage")]
        public void MoveToNextStage(string alias)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            BusinessProcessHelper.MoveToNextStage(aliasRef, _crmContext.Service);
        }

        [When(@"(.*) is updated with the following values")]
        public void WhenAliasIsUpdated(string alias, Table criteria)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _crmContext.TableConverter.ConvertTable(aliasRef.LogicalName, criteria);

            Entity toUpdate = new Entity(aliasRef.LogicalName)
            {
                Id = aliasRef.Id
            };
            foreach (var row in criteria.Rows)
            {
                toUpdate[row["Property"]] = ObjectConverter.ToCrmObject(aliasRef.LogicalName, row["Property"], row["Value"], _crmContext);
            }
            _crmContext.Service.Update(toUpdate);
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

            var request = ObjectConverter.ToSetStateRequest(aliasRef, newStatus, _crmContext);
            _crmContext.Service.Execute<SetStateResponse>(request);
        }

        [When(@"(.*) is deleted")]
        public void WhenAliasIsDeleted(string alias)
        {
            var aliasRef = _crmContext.RecordCache[alias];
            _crmContext.Service.Delete(aliasRef);
            _crmContext.RecordCache.Remove(alias);
        }

        [When(@"(.*) is assigned to (.*)")]
        public void WhenAliasIsAssignedToAlias(string aliasToAssign, string aliasToAssignTo)
        {
            AssignRequest req = new AssignRequest()
            {
                Assignee = _crmContext.RecordCache[aliasToAssignTo],
                Target = _crmContext.RecordCache[aliasToAssign]
            };
            _crmContext.Service.Execute<AssignResponse>(req);

        }

        #endregion

        #region Then


        [Then(@"the process stage of (.*) is (.*)")]
        public void AssertProcessStage(string alias, string stageName)
        {
            var aliasRef = _crmContext.RecordCache[alias];

            var instance = BusinessProcessHelper.GetProcessInstanceOfRecord(aliasRef, _crmContext.Service);
            var stage = BusinessProcessHelper.GetStageByName(instance.GetAttributeValue<EntityReference>("processid"), stageName, _crmContext.Service);

            Assert.AreEqual(instance["processstageid"], stage.Id);
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
