using Microsoft.Xrm.Sdk;
using System;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    public class MoveToNextBusinessProcessStageCommand : BrowserCommand
    {
        private class StageInfo
        {
            public int StageIndex { get; set; }
            public string StageName { get; set; }
        }

        private readonly string _alias;

        public MoveToNextBusinessProcessStageCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext,
            string alias) : base(crmContext, seleniumContext)
        {
            _alias = alias;
        }

        protected override void ExecuteApi()
        {
            var crmRecord = _crmContext.RecordCache[_alias];
            var instance = BusinessProcessFlowHelper.GetProcessInstanceOfRecord(_crmContext, crmRecord);
            var path = BusinessProcessFlowHelper.GetActivePath(_crmContext, instance);

            var stageInfo = GetCurrentStage(instance, path);

            var processRecord = BusinessProcessFlowHelper.GetProcessRecord(instance);
            processRecord["activestageid"] = path[stageInfo.StageIndex + 1].ToEntityReference();
            GlobalTestingContext.ConnectionManager.CurrentConnection.Update(processRecord);
        }

        protected override void ExecuteBrowser()
        {
            var crmRecord = _crmContext.RecordCache[_alias];
            var instance = BusinessProcessFlowHelper.GetProcessInstanceOfRecord(_crmContext, crmRecord);
            var path = BusinessProcessFlowHelper.GetActivePath(_crmContext, instance);
            var stageInfo = GetCurrentStage(instance, path);

            var browser = _seleniumContext.GetBrowser();
            var record = browser.OpenRecord(new OpenFormOptions(crmRecord));
            browser.App.App.BusinessProcessFlow.NextStage(stageInfo.StageName);
            record.Save(true);
        }

        private static StageInfo GetCurrentStage(Entity instance, Entity[] path)
        {
            var stageInfo = new StageInfo()
            {
                StageIndex = -1
            };
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i].Id == instance.GetAttributeValue<Guid>("processstageid"))
                {
                    stageInfo.StageIndex = i;
                    stageInfo.StageName = path[i].GetFormattedValue("stagecategory");
                    break;
                }
            }

            if (stageInfo.StageIndex == -1)
                throw new TestExecutionException(Constants.ErrorCodes.CURRENT_BUSINESS_PROCESS_STAGE_NOT_FOUND);
            if (stageInfo.StageIndex + 1 >= path.Length)
                throw new TestExecutionException(Constants.ErrorCodes.BUSINESS_PROCESS_STAGE_CANNOT_BE_LAST);
            return stageInfo;
        }
    }
}
