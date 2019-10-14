using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.Commands;

namespace Vermaat.Crm.Specflow
{
    public class CommandProcessor
    {
        private readonly ScenarioContext _scenarioContext;

        public TestExecutionException LastError { get; private set; }

        public CommandProcessor(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        public void Execute(ICommand command, CommandAction commandAction = CommandAction.Default)
        {
            try
            {
                Logger.WriteLine($"Executing Command: {command?.GetType().FullName}");
                command.Execute(commandAction);
            }
            catch(TestExecutionException ex)
            {
                if (!HandleException(ex))
                    throw;
            }
        }

        public TResult Execute<TResult>(ICommandFunc<TResult> command, CommandAction commandAction = CommandAction.Default)
        {
            try
            {
                Logger.WriteLine($"Executing Command: {command?.GetType().FullName}");
                return command.Execute(commandAction);
            }
            catch(TestExecutionException ex)
            {
                if (!HandleException(ex))
                    throw;
                else
                    return default(TResult);
            }
        }

        private bool HandleException(TestExecutionException ex)
        {
            LastError = ex;
            if(_scenarioContext.CurrentScenarioBlock != ScenarioBlock.When ||
               !_scenarioContext.ScenarioInfo.Tags.Contains("ExpectedError"))
            {
                return false;
            }

            switch(ex.ErrorCode)
            {
                case Constants.ErrorCodes.FORM_SAVE_FAILED:
                    return true;
                case Constants.ErrorCodes.BUSINESS_PROCESS_ERROR_WHEN_LOADING:
                    return true;

                default: return false;
            }
        }
    }
}
