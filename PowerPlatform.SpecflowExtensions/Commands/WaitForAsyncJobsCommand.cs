using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using PowerPlatform.SpecflowExtensions.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.Models;
using BoDi;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class WaitForAsyncJobsCommand : ApiOnlyCommand
    {
        private readonly string _alias;

        public WaitForAsyncJobsCommand(IObjectContainer container, string alias) : base(container)
        {
            _alias = alias;
        }

        public override void Execute()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];
            int sleepInSeconds = int.Parse(HelperMethods.GetAppSettingsValue("AsyncJobTimeoutInSeconds", true, "30"));

            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (HasOpenSystemJobs(aliasRef.Id))
            {
                Logger.WriteLine("Not all system jobs are completed. Waiting");

                if (timer.Elapsed.TotalSeconds < sleepInSeconds)
                    Thread.Sleep(2000);
                else
                    throw new TestExecutionException(Constants.ErrorCodes.ASYNC_TIMEOUT);
            }
            
            timer.Stop();
            Logger.WriteLine("System jobs are finished");
        }

        private bool HasOpenSystemJobs(Guid regardingId)
        {
            QueryExpression qe = new QueryExpression(AsyncOperation.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(false),
                TopCount = 1
            };
            qe.Criteria.AddCondition(AsyncOperation.Fields.RegardingObjectId, ConditionOperator.Equal, regardingId);
            qe.Criteria.AddCondition(AsyncOperation.Fields.StatusCode, ConditionOperator.NotIn, new object[] { 10, 30, 31, 32 });

            return GlobalContext.ConnectionManager.AdminCrmService.RetrieveMultiple(qe).Entities.Count > 0;
        }
    }
}
