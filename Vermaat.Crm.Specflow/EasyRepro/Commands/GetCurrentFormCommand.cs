using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.CommonModels;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    internal class GetCurrentFormCommand : ISeleniumCommandFunc<SystemForm>
    {
        private readonly bool _isQuickCreate;

        public GetCurrentFormCommand(bool isQuickCreate)
        {
            _isQuickCreate = isQuickCreate;
        }

        public CommandResult<SystemForm> Execute(BrowserInteraction browserInteraction)
        {
            Guid formId = Guid.Empty;

            if (_isQuickCreate)
            {
                var formIdElement = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_QuickCreate_DialogRoot), TimeSpan.FromSeconds(5));
                if (formIdElement == null)
                    return CommandResult<SystemForm>.Fail(true, Constants.ErrorCodes.FORMID_NOT_FOUND);
                Logger.WriteLine("Quick create form available");

                formId = Guid.Parse(formIdElement.GetAttribute("data-preview-id"));
            }
            else
            {
                var formIdElement = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormId), TimeSpan.FromSeconds(5));
                if (formIdElement == null)
                {
                    Logger.WriteLine("Getting form via script");
                    var formIdScripted = browserInteraction.Driver.ExecuteScript("return Xrm.Page.data.entity.getId();") as string;
                    if(string.IsNullOrEmpty(formIdScripted))
                    {
                        return CommandResult<SystemForm>.Fail(true, Constants.ErrorCodes.FORMID_NOT_FOUND);
                    }
                    formId = Guid.Parse(formIdScripted);

                }
               
                Logger.WriteLine("Form available");


                var route = formIdElement.GetAttribute("route");
                Logger.WriteLine($"Determining form: {route}");

                formId = Guid.Parse(route.Substring(route.LastIndexOf('/') + 1));
            }

            return CommandResult<SystemForm>.Success(SystemForm.GetById(GlobalTestingContext.ConnectionManager.CurrentConnection, formId));
        }
    }
}
