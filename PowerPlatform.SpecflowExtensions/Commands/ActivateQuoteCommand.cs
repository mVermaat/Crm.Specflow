using BoDi;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using PowerPlatform.SpecflowExtensions.EasyRepro;
using PowerPlatform.SpecflowExtensions.EasyRepro.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class ActivateQuoteCommand : BrowserCommand
    {
        private readonly string _alias;

        public ActivateQuoteCommand(IObjectContainer container, string alias)
            : base(container)
        {
            _alias = alias;
        }

        protected override void ExecuteApi()
        {
            var aliasRef = _crmContext.RecordCache[_alias];

            var toUpdate = new Entity(aliasRef.LogicalName, aliasRef.Id);
            toUpdate["statecode"] = new OptionSetValue(1); //Active
            toUpdate["statuscode"] = new OptionSetValue(2); //In progress

            GlobalContext.ConnectionManager.CurrentCrmService.Update(toUpdate);
        }

        protected override void ExecuteBrowser()
        {
            EntityReference aliasRef = _crmContext.RecordCache[_alias];

            var browser = GlobalContext.ConnectionManager.GetCurrentBrowserSession(_seleniumContext);
            browser.GetApp<CustomerEngagementApp>(_container).OpenRecord(new OpenFormOptions(aliasRef));
            browser.GetApp<QuoteActions>(_container).ActivateQuote();

            

           
        }
    }
}
