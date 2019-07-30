using System;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Commands
{
    public abstract class BrowserCommandFunc<TResult> : ICommandFunc<TResult>
    {
        protected readonly CrmTestingContext _crmContext;
        protected readonly SeleniumTestingContext _seleniumContext;

        public BrowserCommandFunc(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        public TResult Execute(CommandAction commandAction = CommandAction.Default)
        {
            if (commandAction == CommandAction.ForceApi || _crmContext.IsTarget(Constants.SpecFlow.TARGET_API))
            {
                return ExecuteApi();
            }
            else if (commandAction == CommandAction.ForceBrowser ||
                     _crmContext.IsTarget(Constants.SpecFlow.TARGET_Chrome) ||
                     _crmContext.IsTarget(Constants.SpecFlow.TARGET_Edge) ||
                     _crmContext.IsTarget(Constants.SpecFlow.TARGET_Firefox) ||
                     _crmContext.IsTarget(Constants.SpecFlow.TARGET_InternetExplorer))
            {
                return ExecuteBrowser();
            }
            else
            {
                throw new TestExecutionException(Constants.ErrorCodes.UNKNOWN_TAG);
            }
        }

        protected abstract TResult ExecuteApi();
        protected abstract TResult ExecuteBrowser();
    }

    public abstract class BrowserCommand : ICommand
    {
        protected readonly CrmTestingContext _crmContext;
        protected readonly SeleniumTestingContext _seleniumContext;

        public BrowserCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        public void Execute(CommandAction commandAction = CommandAction.Default)
        {
            if (commandAction == CommandAction.ForceApi ||
                _crmContext.IsTarget(Constants.SpecFlow.TARGET_API))
            {
                ExecuteApi();
            }
            else if (commandAction == CommandAction.ForceBrowser ||
                     _crmContext.IsTarget(Constants.SpecFlow.TARGET_Chrome) ||
                     _crmContext.IsTarget(Constants.SpecFlow.TARGET_Edge) ||
                     _crmContext.IsTarget(Constants.SpecFlow.TARGET_Firefox) ||
                     _crmContext.IsTarget(Constants.SpecFlow.TARGET_InternetExplorer))
            {
                ExecuteBrowser();
            }
            else
            {
                throw new TestExecutionException(Constants.ErrorCodes.UNKNOWN_TAG);
            }
        }

        protected abstract void ExecuteApi();
        protected abstract void ExecuteBrowser();
    }
}
