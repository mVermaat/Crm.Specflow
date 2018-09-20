using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Processors;

namespace Vermaat.Crm.Specflow
{
    public class StepProcessor
    {
        public ICrmStepProcessor GeneralCrm { get; set; }
        public IBusinessProcessFlowProcessor BusinessProcessFlow { get; set; }
        public ILeadProcessor LeadProcessor { get; set; }

        public void SetDefaultProcessors(CrmTestingContext crmContext, bool overruleCurrentProcessors = false)
        {
            if (BusinessProcessFlow == null || overruleCurrentProcessors)
                BusinessProcessFlow = new BusinessProcessFlowProcessor(crmContext);

            if (GeneralCrm == null || overruleCurrentProcessors)
                GeneralCrm = new CrmStepProcessor(crmContext);

            if (LeadProcessor == null || overruleCurrentProcessors)
                LeadProcessor = new LeadProcessor(crmContext);
        }
    }
}
