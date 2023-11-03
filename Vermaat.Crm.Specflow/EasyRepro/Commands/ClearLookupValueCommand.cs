using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class ClearLookupValueCommand : ISeleniumCommand
    {
        private readonly LookupItem _lookupItem;

        public ClearLookupValueCommand(LookupItem lookupItem)
        {
            _lookupItem = lookupItem;
        }

        public CommandResult Execute(BrowserInteraction browserInteraction)
        {
            var controlName = _lookupItem.Name;

            var fieldContainer = browserInteraction.Driver.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupFieldContainer].Replace("[NAME]", controlName)));
            fieldContainer.Hover(browserInteraction.Driver);

            var xpathDeleteExistingValues = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldDeleteExistingValue].Replace("[NAME]", controlName));
            var existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);

            var xpathToExpandButton = By.XPath(AppElements.Xpath[AppReference.Entity.LookupFieldExpandCollapseButton].Replace("[NAME]", controlName));
            bool success = fieldContainer.TryFindElement(xpathToExpandButton, out var expandButton);
            if (success)
            {
                expandButton.Click(true);

                var count = existingValues.Count;
                fieldContainer.WaitUntil(x => x.FindElements(xpathDeleteExistingValues).Count > count);
            }

            fieldContainer.WaitUntilAvailable(By.XPath(AppElements.Xpath[AppReference.Entity.TextFieldLookupSearchButton].Replace("[NAME]", controlName)));

            existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);
            if (existingValues.Count == 0)
                return CommandResult.Success();


            // Removes all selected items
            while (existingValues.Count > 0)
            {
                foreach (var v in existingValues)
                    v.Click(true);

                existingValues = fieldContainer.FindElements(xpathDeleteExistingValues);
            }

            return CommandResult.Success();
            
        }
    }
}
