using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class CommandResult
    {
        private CommandResult()
        {
            
        }

        public bool AllowRetry { get; private set; }

        public int ErrorCode { get; private set; }

        public object[] ErrorMessageFormatArgs { get; private set; }

        public bool IsSuccessfull { get; private set; }

        public static CommandResult Success() 
            => new CommandResult() 
            { 
                IsSuccessfull = true 
            }; 
        
        public static CommandResult Fail(bool allowRetry, int errorCode, params object[] formatArgs) 
            => new CommandResult() 
            { 
                AllowRetry = allowRetry,
                ErrorCode = errorCode,
                ErrorMessageFormatArgs = formatArgs,
                IsSuccessfull = false                
            }; 
    }
}
