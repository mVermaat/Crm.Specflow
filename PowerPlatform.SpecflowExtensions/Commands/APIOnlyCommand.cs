using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public abstract class ApiOnlyCommandFunc<TResult> : ICommandFunc<TResult>
    {
        protected readonly ICrmContext _crmContext;

        public ApiOnlyCommandFunc(ICrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        public abstract TResult Execute();

        public TResult Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget)
        {
            var target = prefferedTarget ?? targetCalculator.Calculate(_crmContext, this);
            if(target == CommandTarget.ForceBrowser)
                throw new TestExecutionException(Constants.ErrorCodes.BROWSER_OT_SUPPORTED_FOR_API_TEST);
            return Execute();
        }
    }

    public abstract class ApiOnlyCommand : ICommand
    {
        protected readonly ICrmContext _crmContext;

        public ApiOnlyCommand(ICrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        public abstract void Execute();

        public void Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget)
        {
            var target = prefferedTarget ?? targetCalculator.Calculate(_crmContext, this);
            if (target == CommandTarget.ForceBrowser)
                throw new TestExecutionException(Constants.ErrorCodes.BROWSER_OT_SUPPORTED_FOR_API_TEST);
            Execute();
        }
    }
}
