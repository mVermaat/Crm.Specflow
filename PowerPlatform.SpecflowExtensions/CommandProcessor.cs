using PowerPlatform.SpecflowExtensions.Commands;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions
{
    public sealed class CommandProcessor
    {
        private readonly ScenarioContext _scenarioContext;

        internal TestExecutionException LastError { get; private set; }
        public ICommandTargetCalculator CommandTargetCalculator { get; set; }

        internal CommandProcessor(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            CommandTargetCalculator = new DefaultCommandTargetCalculator();
        }

        public void Execute(ICommand command, CommandTarget? target = null)
        {
            try
            {
                Logger.WriteLine($"Executing Command: {command?.GetType().FullName}");
                command.Execute(CommandTargetCalculator, target);
            }
            catch (TestExecutionException ex)
            {
                if (!HandleException(ex))
                    throw;
            }
        }

        public TResult Execute<TResult>(ICommandFunc<TResult> command, CommandTarget? target = null)
        {
            try
            {
                Logger.WriteLine($"Executing Command: {command?.GetType().FullName}");
                return command.Execute(CommandTargetCalculator, target);
            }
            catch (TestExecutionException ex)
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
            if (_scenarioContext.CurrentScenarioBlock != ScenarioBlock.When ||
               !_scenarioContext.ScenarioInfo.Tags.Contains("ExpectedError"))
            {
                return false;
            }

            switch (ex.ErrorCode)
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
