using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class CommandResult
    {
        public CommandResult()
        {
            AllowRetry = true;
        }

        public bool AllowRetry { get; set; }
        public bool IsSuccessfull { get; set; }
        public string Message { get; set; }
    }
}
