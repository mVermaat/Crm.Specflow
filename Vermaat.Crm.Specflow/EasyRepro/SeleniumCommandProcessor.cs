using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro.Commands;
using static Vermaat.Crm.Specflow.Constants;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public static class SeleniumCommandProcessor
    {
        private static readonly SeleniumSelectorData _selectors;

        static SeleniumCommandProcessor()
        {
            _selectors = new SeleniumSelectorData();
        }

        public static void ExecuteCommand(IWebDriver driver, ISeleniumCommand command)
        {
            ExecuteCommand(driver, command, 2);
        }

        public static T ExecuteCommand<T>(IWebDriver driver, ISeleniumCommandFunc<T> command)
        {
            return ExecuteCommand(driver, command, 2);
        }

        private static void ExecuteCommand(IWebDriver driver, ISeleniumCommand command, int retries)
        {
            try
            {
                Logger.WriteLine($"Executing Selenium Command {command.GetType().Name}. Retries left: {retries}");
                var result = command.Execute(driver, _selectors);

                if(result == null)
                    throw new TestExecutionException(Constants.ErrorCodes.SELENIUM_COMMAND_NO_RESULT, command.GetType().Name);


                if (!result.IsSuccessfull)
                {
                    if(result.AllowRetry && retries > 0)
                        ExecuteCommand(driver, command, retries - 1);
                    else
                        throw new TestExecutionException(Constants.ErrorCodes.SELENIUM_COMMAND_FAILED, command.GetType().Name, GlobalTestingContext.ErrorCodes.GetErrorMessage(result.ErrorCode, result.ErrorMessageFormatArgs));
                }
            }
            catch(Exception ex)
            {
                if (retries > 0)
                    ExecuteCommand(driver, command, retries - 1);
                else
                    throw new TestExecutionException(Constants.ErrorCodes.SELENIUM_COMMAND_FAILED, ex, command.GetType().Name, $"{ex.GetType().Name} with message: {ex.Message}");
            }

            Logger.WriteLine($"Executing Selenium Command {command.GetType().Name} successfull");
        }

        private static T ExecuteCommand<T>(IWebDriver driver, ISeleniumCommandFunc<T> command, int retries)
        {
            try
            {
                Logger.WriteLine($"Executing Selenium Command {command.GetType().Name}. Retries left: {retries}");
                var result = command.Execute(driver, _selectors);

                if (result == null)
                    throw new TestExecutionException(Constants.ErrorCodes.SELENIUM_COMMAND_NO_RESULT, command.GetType().Name);


                if (!result.IsSuccessfull)
                {
                    if (result.AllowRetry && retries > 0)
                        return ExecuteCommand(driver, command, retries - 1);
                    else
                        throw new TestExecutionException(Constants.ErrorCodes.SELENIUM_COMMAND_FAILED, command.GetType().Name, GlobalTestingContext.ErrorCodes.GetErrorMessage(result.ErrorCode, result.ErrorMessageFormatArgs));
                }

                Logger.WriteLine($"Executing Selenium Command {command.GetType().Name} successfull");
                return result.Result;

            }
            catch (Exception ex)
            {
                if (retries > 0)
                    return ExecuteCommand(driver, command, retries - 1);
                else
                    throw new TestExecutionException(Constants.ErrorCodes.SELENIUM_COMMAND_FAILED, ex, command.GetType().Name, $"{ex.GetType().Name} with message: {ex.Message}");
            }

        }
    }
}
