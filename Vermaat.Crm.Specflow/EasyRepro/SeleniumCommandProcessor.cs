using OpenQA.Selenium;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static void ExecuteCommand(UCIApp app, ISeleniumCommand command)
        {
            ExecuteCommand(GetBrowserInteraction(app), command, 2);
        }

        public static void ExecuteCommand(BrowserInteraction browserInteraction, ISeleniumCommand command)
        {
            ExecuteCommand(browserInteraction, command, 2);
        }

        public static T ExecuteCommand<T>(UCIApp app, ISeleniumCommandFunc<T> command)
        {
            return ExecuteCommand(GetBrowserInteraction(app), command, 2);
        }

        public static T ExecuteCommand<T>(BrowserInteraction browserInteraction, ISeleniumCommandFunc<T> command)
        {
            return ExecuteCommand(browserInteraction, command, 2);
        }

        private static void ExecuteCommand(BrowserInteraction browserInteraction, ISeleniumCommand command, int retries)
        {
            Logger.WriteLine($"Executing Selenium Command {command.GetType().Name}. Retries left: {retries}");
            var result = command.Execute(browserInteraction);

            if(result == null)
                throw new TestExecutionException(Constants.ErrorCodes.SELENIUM_COMMAND_NO_RESULT, command.GetType().Name);


            if (!result.IsSuccessfull)
            {
                if (result.AllowRetry && retries > 0)
                {
                    Delay();
                    ExecuteCommand(browserInteraction, command, retries - 1);
                }
                else
                    throw new TestExecutionException(result.ErrorCode, result.ErrorMessageFormatArgs);
            }
            Logger.WriteLine($"Executing Selenium Command {command.GetType().Name} successfull");
        }

        private static T ExecuteCommand<T>(BrowserInteraction browserInteraction, ISeleniumCommandFunc<T> command, int retries)
        {
            Logger.WriteLine($"Executing Selenium Command {command.GetType().Name}. Retries left: {retries}");
            var result = command.Execute(browserInteraction);

            if (result == null)
                throw new TestExecutionException(Constants.ErrorCodes.SELENIUM_COMMAND_NO_RESULT, command.GetType().Name);


            if (!result.IsSuccessfull)
            {
                if (result.AllowRetry && retries > 0)
                {
                    Delay();
                    return ExecuteCommand(browserInteraction, command, retries - 1);
                }
                else
                    throw new TestExecutionException(result.ErrorCode, result.ErrorMessageFormatArgs);
            }

            Logger.WriteLine($"Executing Selenium Command {command.GetType().Name} successfull");
            return result.Result;
        }

        private static void Delay()
        {
            Logger.WriteLine("Waiting for a moment before next try");
            Thread.Sleep(1000);
        }

        private static BrowserInteraction GetBrowserInteraction(UCIApp app)
            => new BrowserInteraction(app.WebDriver, _selectors, app.SeleniumCommandFactory, app.LocalizedTexts, app.UILanguageCode);
    }
}
