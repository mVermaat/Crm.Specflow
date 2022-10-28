using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class ClickRibbonItemCommand : ISeleniumCommand
    {
        private readonly string _buttonName;

        internal ClickRibbonItemCommand(string buttonName) 
        {
            _buttonName = buttonName;
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            var result = SeleniumCommandProcessor.ExecuteCommand(browserInteraction,
                browserInteraction.SeleniumCommandFactory.CreateGetRibbonItemCommand(_buttonName));

            if (result != null)
            {
                result.Click();
                return CommandResult.Success();
            }
            else
                return CommandResult.Fail(true, Constants.ErrorCodes.RIBBON_BUTTON_DOESNT_EXIT, _buttonName);
        }
    }
}
