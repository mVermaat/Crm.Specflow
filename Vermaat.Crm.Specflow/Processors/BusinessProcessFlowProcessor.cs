using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Processors
{
    public class BusinessProcessFlowProcessor : IBusinessProcessFlowProcessor
    {
        private readonly CrmTestingContext _crmContext;

        public BusinessProcessFlowProcessor(CrmTestingContext context)
        {
            this._crmContext = context;
        }

        public void MoveToStage(EntityReference crmRecord, string stageName)
        {
            BusinessProcessHelper.MoveToStage(crmRecord, stageName, _crmContext.Service);
        }

        public void MoveNext(EntityReference crmRecord)
        {
            BusinessProcessHelper.MoveToNextStage(crmRecord, _crmContext.Service);
        }

        public ProcessStage GetCurrentStage(EntityReference aliasRef)
        {
            var instance = BusinessProcessHelper.GetProcessInstanceOfRecord(aliasRef, _crmContext.Service);

            return new ProcessStage
            {
                ProcessId = instance.GetAttributeValue<EntityReference>("processid").Id,
                StageId = instance.GetAttributeValue<Guid>("processstageid")
            };
        }

        public Guid GetStageByName(Guid processId, string stageName)
        {
           return BusinessProcessHelper.GetStageByName(processId, stageName, _crmContext.Service).Id;
        }
    }
}
