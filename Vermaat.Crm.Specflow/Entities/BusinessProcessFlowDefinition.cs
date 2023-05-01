using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vermaat.Crm.Specflow.Entities
{
    public class BusinessProcessFlowDefinition
    {
        [JsonProperty(PropertyName = "BusinessProcessFlowId")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "BusinessProcessFlowName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "BusinessProcessFlowUniqueName")]
        public string UniqueName { get; set; }

        [JsonProperty(PropertyName = "BusinessProcessFlowEntities")]
        public BusinessProcessFlowEntity[] Entities { get; set; }
    }

    public class BusinessProcessFlowEntity
    {
        public string EntityLogicalName { get; set; }

        public BusinessProcessFlowStage Stage { get; set; }
    }

    public class BusinessProcessFlowStage
    {
        public string StageLogicalName { get; set; }
        public string StageDisplayName { get; set; }
        public string StageId { get; set; }
        public BusinessProcessFlowStep[] Steps { get; set; }

    }

    public class BusinessProcessFlowStep
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public BusinessProcessFlowStepType StepType { get; set; }
        public string StepControlId { get; set; }
    }

    public enum BusinessProcessFlowStepType
    {
        Field,
    }
}
