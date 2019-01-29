using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow
{
    public class CommandProcessor
    {
        public void Execute(ICommand command)
        {
            command.Execute();
        }

        public TResult Execute<TResult>(ICommandFunc<TResult> command)
        {
            return command.Execute();
        }
    }
}
