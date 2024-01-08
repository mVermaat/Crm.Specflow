using System;
using System.Threading;

namespace Vermaat.Crm.Specflow.Commands
{
    public class TryUntilCommand<TCommand> : ICommand where TCommand : ICommand
    {
        private readonly CrmTestingContext _crmContext;
        private readonly TCommand _childCommand;
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _waitPeriod;

        public TryUntilCommand(CrmTestingContext crmContext, TCommand childCommand, TimeSpan timeout, TimeSpan waitPeriod)
        {
            _crmContext = crmContext;
            _childCommand = childCommand;
            _timeout = timeout;
            _waitPeriod = waitPeriod;
        }

        public void Execute(CommandAction commandAction)
        {
            DateTime timeoutDateTime = DateTime.Now.Add(_timeout);
            ExecuteUntil(timeoutDateTime, commandAction);

        }

        private void ExecuteUntil(DateTime timeoutDateTime, CommandAction commandAction)
        {
            try
            {
                _crmContext.CommandProcessor.Execute(_childCommand, commandAction);
            }
            catch (Exception)
            {
                if (DateTime.Now < timeoutDateTime)
                {
                    Thread.Sleep((int)_waitPeriod.TotalMilliseconds);
                    ExecuteUntil(timeoutDateTime, commandAction);
                }
                else
                    throw;
            }
        }
    }

    public class TryUntilCommandFunc<TCommand, TResult> : ICommandFunc<TResult> where TCommand : ICommandFunc<TResult>
    {
        private readonly CrmTestingContext _crmContext;
        private readonly TCommand _childCommand;
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _waitPeriod;
        private readonly Func<TResult, bool> _assertResultValid;
        private readonly Func<TResult, string> _timeoutMessageFunc;

        public TryUntilCommandFunc(CrmTestingContext crmContext, TCommand childCommand, TimeSpan timeout,
            TimeSpan waitPeriod, Func<TResult, bool> assertResultValid, Func<TResult, string> timeoutMessageFunc)
        {
            _crmContext = crmContext;
            _childCommand = childCommand;
            _timeout = timeout;
            _waitPeriod = waitPeriod;
            _assertResultValid = assertResultValid;
            _timeoutMessageFunc = timeoutMessageFunc;
        }

        public TResult Execute(CommandAction commandAction)
        {
            DateTime timeoutDateTime = DateTime.Now.Add(_timeout);

            return ExecuteUntil(timeoutDateTime, commandAction);
        }

        private TResult ExecuteUntil(DateTime timeoutDateTime, CommandAction commandAction)
        {
            try
            {
                var result = _crmContext.CommandProcessor.Execute(_childCommand, commandAction);

                if (_assertResultValid(result))
                    return result;
                else if (DateTime.Now < timeoutDateTime)
                {
                    Thread.Sleep((int)_waitPeriod.TotalMilliseconds);
                    return ExecuteUntil(timeoutDateTime, commandAction);
                }
                else
                    throw new TestExecutionException(Constants.ErrorCodes.TRYUNTIL_TIMEOUT, _timeoutMessageFunc(result));

            }
            catch (Exception)
            {
                if (DateTime.Now < timeoutDateTime)
                {
                    Thread.Sleep((int)_waitPeriod.TotalMilliseconds);
                    return ExecuteUntil(timeoutDateTime, commandAction);
                }
                else
                    throw;
            }
        }
    }
}
