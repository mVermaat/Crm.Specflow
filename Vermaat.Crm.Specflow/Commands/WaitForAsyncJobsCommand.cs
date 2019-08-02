using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            int tryCount = 0;
            while (tryCount < 15 && QueryHelper.HasOpenSystemJobs(aliasRef.Id, GlobalTestingContext.ConnectionManager.AdminConnection))
            {
                Logger.WriteLine("Not all system jobs are completed. Waiting");
                Thread.Sleep(2000);
                tryCount++;
            }
            Logger.WriteLine("System jobs are finished");

            Assert.AreNotEqual(15, tryCount, "Not all system jobs were finished on time");
        }
    }
}
