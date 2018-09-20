using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow
{
    public static class ScenarioContextExtensions
    {
        public static bool IsTagTargetted(this ScenarioContext context, string target)
        {
            return target.Equals(ConfigurationManager.AppSettings["Target"], StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
