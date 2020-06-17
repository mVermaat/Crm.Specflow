using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public abstract class BrowserCommandFunc<TResult> : ICommandFunc<TResult>
    {
        protected readonly ICrmContext _crmContext;
        protected readonly ISeleniumContext _seleniumContext;

        public BrowserCommandFunc(ICrmContext crmContext, ISeleniumContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        public TResult Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget)
        {
            var target = prefferedTarget ?? targetCalculator.Calculate(_crmContext, this);

            switch (target)
            {
                case CommandTarget.ForceApi:
                case CommandTarget.PreferApi:
                    return ExecuteApi();
                case CommandTarget.PreferBrowser:
                case CommandTarget.ForceBrowser:
                    return ExecuteBrowser();
                default:
                    throw new TestExecutionException(Constants.ErrorCodes.UNKNOWN_TAG);
            }
        }

        protected abstract TResult ExecuteApi();
        protected abstract TResult ExecuteBrowser();
    }

    public abstract class BrowserCommand : ICommand
    {
        protected readonly ICrmContext _crmContext;
        protected readonly ISeleniumContext _seleniumContext;

        public BrowserCommand(ICrmContext crmContext, ISeleniumContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        public void Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget)
        {
            var target = prefferedTarget ?? targetCalculator.Calculate(_crmContext, this);

            switch (target)
            {
                case CommandTarget.ForceApi:
                case CommandTarget.PreferApi:
                    ExecuteApi(); break;
                case CommandTarget.PreferBrowser:
                case CommandTarget.ForceBrowser:
                    ExecuteBrowser(); break;
                default:
                    throw new TestExecutionException(Constants.ErrorCodes.UNKNOWN_TAG);
            }
        }

        protected abstract void ExecuteApi();
        protected abstract void ExecuteBrowser();
    }
}
