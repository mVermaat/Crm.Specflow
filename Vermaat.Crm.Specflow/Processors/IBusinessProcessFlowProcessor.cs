using System;
using Microsoft.Xrm.Sdk;

namespace Vermaat.Crm.Specflow.Processors
{
    public interface IBusinessProcessFlowProcessor
    {
        ProcessStage GetCurrentStage(EntityReference aliasRef);
        Guid? GetStageByName(Guid processId, string stageName);
        void MoveNext(EntityReference crmRecord);
        void MoveToStage(EntityReference crmRecord, string stageName);
    }
}