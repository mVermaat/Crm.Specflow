using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow
{
    public class CommandProcessor
    {
        public void Execute(ICommand command, CommandAction commandAction = CommandAction.Default)
        {
            command.Execute(commandAction);
        }

        public TResult Execute<TResult>(ICommandFunc<TResult> command, CommandAction commandAction = CommandAction.Default)
        {
            Logger.WriteLine($"Executing Command: {command?.GetType().FullName}");
            return command.Execute(commandAction);
        }
    }
}
