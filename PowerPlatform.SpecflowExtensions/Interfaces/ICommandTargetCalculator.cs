using PowerPlatform.SpecflowExtensions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Interfaces
{
    public interface ICommandTargetCalculator
    {
        CommandTarget Calculate(ICrmContext crmContext, ICommand command);
        CommandTarget Calculate<TResult>(ICrmContext crmContext, ICommandFunc<TResult> command);
    }
}
