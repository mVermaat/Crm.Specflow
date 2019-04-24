using System;

namespace Vermaat.Crm.Specflow.Commands
{
    public abstract class BrowserOnlyCommandFunc<TResult> : ICommandFunc<TResult>
    {
        protected readonly CrmTestingContext _crmContext;
        private readonly SeleniumTestingContext _seleniumContext;

        public BrowserOnlyCommandFunc(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        public abstract TResult Execute();

        public TResult Execute(CommandAction commandAction = CommandAction.Default)
        {
            if (commandAction == CommandAction.ForceApi)
                throw new InvalidOperationException("API not supported for Browser only commands");
            return Execute();
        }
    }


    public abstract class BrowserOnlyCommand : ICommand
    {
        protected readonly CrmTestingContext _crmContext;
        protected readonly SeleniumTestingContext _seleniumContext;

        public BrowserOnlyCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext)
        {
            _crmContext = crmContext;
            _seleniumContext = seleniumContext;
        }

        public abstract void Execute();

        public void Execute(CommandAction commandAction = CommandAction.Default)
        {
            if (commandAction == CommandAction.ForceApi)
                throw new InvalidOperationException("API not supported for Browser only commands");
            Execute();
        }
    }
}
