using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public interface ICommandFunc<TResult>
    {
        TResult Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget);
    }
}
