using BoDi;
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
        protected readonly IObjectContainer _container;

        public ApiOnlyCommandFunc(IObjectContainer container)
        {
            _crmContext = container.Resolve<ICrmContext>();
            _container = container;
        }

        public abstract TResult Execute();

        public TResult Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget)
        {
            var target = prefferedTarget ?? targetCalculator.Calculate(_crmContext, this);
            if(target == CommandTarget.ForceBrowser)
                throw new TestExecutionException(Constants.ErrorCodes.BROWSER_NOT_SUPPORTED_FOR_API_TEST);
            return Execute();
        }
    }

    public abstract class ApiOnlyCommand : ICommand
    {
        protected readonly ICrmContext _crmContext;
        protected readonly IObjectContainer _container;

        public ApiOnlyCommand(IObjectContainer container)
        {
            _crmContext = container.Resolve<ICrmContext>();
            _container = container;
        }

        public abstract void Execute();

        public void Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget)
        {
            var target = prefferedTarget ?? targetCalculator.Calculate(_crmContext, this);
            if (target == CommandTarget.ForceBrowser)
                throw new TestExecutionException(Constants.ErrorCodes.BROWSER_NOT_SUPPORTED_FOR_API_TEST);
            Execute();
        }
    }
}
