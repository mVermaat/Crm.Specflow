using BoDi;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PowerPlatform.SpecflowExtensions.Commands;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class RunOnDemandWorkflowCommand : ApiOnlyCommand
    {
        private readonly string _workflowName;
        private readonly string _alias;

        public RunOnDemandWorkflowCommand(IObjectContainer container, string workflowName, string alias) : base(container)
        {
            _workflowName = workflowName;
            _alias = alias;
        }

        public override void Execute()
        {
            var workflow = GetWorkflow();
            GlobalContext.ConnectionManager.CurrentCrmService.Execute<ExecuteWorkflowResponse>(new ExecuteWorkflowRequest()
            {
                WorkflowId = workflow.Id,
                EntityId = _crmContext.RecordCache.Get(_alias, true).Id
            });

            // Wait for completion if it is async
            if (workflow.GetAttributeValue<OptionSetValue>("mode")?.Value == 0)
                _crmContext.CommandProcessor.Execute(new WaitForAsyncJobsCommand(_container, _alias));
        }

        private Entity GetWorkflow()
        {
            var workflows = GlobalContext.ConnectionManager.CurrentCrmService.RetrieveMultiple(new QueryExpression("workflow")
            {
                ColumnSet = new ColumnSet("mode"),
                NoLock = true,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("type", ConditionOperator.Equal, 1),
                        new ConditionExpression("category", ConditionOperator.Equal, 0),
                        new ConditionExpression("name", ConditionOperator.Equal, _workflowName),
                    }
                }
            }).Entities;

            if (workflows.Count != 1)
                throw new TestExecutionException(Constants.ErrorCodes.UNEXPECTED_PROCESS_COUNT, _workflowName, workflows.Count);

            return workflows[0];
        }
    }
}