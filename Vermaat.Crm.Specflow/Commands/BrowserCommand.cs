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

        public TResult Execute()
        {
            if (ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_API))
            {
                return ExecuteApi();
            }
            else if (ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_Chrome) ||
                     ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_Edge) ||
                     ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_Firefox) ||
                     ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_InternetExplorer))
            {
                return ExecuteBrowser();
            }
            else
            {
                throw new InvalidOperationException("Unknown tag. Use API, Chrome, Edge, Firefox or IE");
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

        public void Execute()
        {
            if (ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_API))
            {
                ExecuteApi();
            }
            else if (ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_Chrome) ||
                     ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_Edge) ||
                     ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_Firefox) ||
                     ScenarioContext.Current.IsTagTargetted(Constants.SpecFlow.TARGET_InternetExplorer))
            {
                ExecuteBrowser();
            }
            else
            {
                throw new InvalidOperationException("Unknown tag. Use API, Chrome, Edge, Firefox or IE");
            }
        }

        protected abstract void ExecuteApi();
        protected abstract void ExecuteBrowser();
    }
}
