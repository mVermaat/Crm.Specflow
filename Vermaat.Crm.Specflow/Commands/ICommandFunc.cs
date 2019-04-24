using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Commands
{
    public interface ICommandFunc<TResult>
    {
        TResult Execute(CommandAction commandAction);
    }
}
