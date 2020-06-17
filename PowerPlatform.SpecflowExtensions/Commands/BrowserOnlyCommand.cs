using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public abstract class BrowserOnlyCommandFunc<TResult> : ICommandFunc<TResult>
    {
        protected readonly ICrmContext _crmContext;
        protected readonly ISeleniumContext _seleniumContext;

        public BrowserOnlyCommandFunc(ICrmContext crmContext, ISeleniumContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        public TResult Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget)
        {
            var target = prefferedTarget ?? targetCalculator.Calculate(_crmContext, this);

            if(target == CommandTarget.PreferApi || target == CommandTarget.ForceApi)
                throw new TestExecutionException(Constants.ErrorCodes.API_NOT_SUPPORTED_FOR_BROWSER_ONLY_COMMANDS);

            return Execute();
        }

        public abstract TResult Execute();
    }


    public abstract class BrowserOnlyCommand : ICommand
    {
        protected readonly ICrmContext _crmContext;
        protected readonly ISeleniumContext _seleniumContext;

        public BrowserOnlyCommand(ICrmContext crmContext, ISeleniumContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        public abstract void Execute();

        public void Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget)
        {
            var target = prefferedTarget ?? targetCalculator.Calculate(_crmContext, this);

            if (target == CommandTarget.PreferApi || target == CommandTarget.ForceApi)
                throw new TestExecutionException(Constants.ErrorCodes.API_NOT_SUPPORTED_FOR_BROWSER_ONLY_COMMANDS);

            Execute();
        }
    }
}
