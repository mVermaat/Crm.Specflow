using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

namespace Vermaat.Crm.Specflow.Commands
{
    public class WaitForAsyncJobsCommand : ApiOnlyCommand
    {
        private readonly string _alias;

        public WaitForAsyncJobsCommand(CrmTestingContext crmContext, string alias) : base(crmContext)
        {
            _alias = alias;
        }

        public override void Execute()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];
            int sleepInSeconds = int.Parse(HelperMethods.GetAppSettingsValue("AsyncJobTimeoutInSeconds", true, "30"));

            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (QueryHelper.HasOpenSystemJobs(aliasRef.Id, GlobalTestingContext.ConnectionManager.AdminConnection))
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
    }
}
