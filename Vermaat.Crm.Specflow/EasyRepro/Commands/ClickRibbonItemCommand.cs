using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class ClickRibbonItemCommand : GetRibbonItemCommand
    {
        private readonly string _buttonName;

        public ClickRibbonItemCommand(string buttonName) 
            : base(buttonName)
        {
            _buttonName = buttonName;
        }

        public override CommandResult<IWebElement> Execute(IWebDriver driver, SeleniumSelectorData selectors)
        {
            var result = base.Execute(driver, selectors);

            if (result == null || !result.IsSuccessfull)
                return result;

            if (result.Result != null)
            {
                result.Result.Click();
                return result;
            }
            else
                return CommandResult<IWebElement>.Fail(true, Constants.ErrorCodes.RIBBON_BUTTON_DOESNT_EXIT, _buttonName);
        }
    }
}
