using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.Generic;
using System.Linq;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class GetCompositeControlFieldsCommand : ISeleniumCommandFunc<IEnumerable<string>>
    {
        private readonly string _compositeControlAttributeName;

        public GetCompositeControlFieldsCommand(string compositeControlAttributeName)
        {
            _compositeControlAttributeName = compositeControlAttributeName;
        }

        public CommandResult<IEnumerable<string>> Execute(BrowserInteraction browserInteraction)
        {
            var xPathSelector = browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_CompositeControls, _compositeControlAttributeName);

            // First wait in case the form isn't loaded yet
            var found = browserInteraction.Driver.WaitUntilAvailable(xPathSelector);
            if (found == null)
                return CommandResult<IEnumerable<string>>.Fail(true, Constants.ErrorCodes.COMPOSITE_CONTROL_NOT_FOUND, _compositeControlAttributeName);

            var elements = browserInteraction.Driver.FindElements(xPathSelector)
                .Select(e => e.GetAttribute("data-control-name").Substring(_compositeControlAttributeName.Length + 24));

            return CommandResult<IEnumerable<string>>.Success(elements);

        }
    }
}
