﻿using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    internal class GetCurrentFormCommand : ISeleniumCommandFunc<SystemForm>
    {
        public CommandResult<SystemForm> Execute(BrowserInteraction browserInteraction)
        {
            var formIdElement = browserInteraction.Driver.WaitUntilAvailable(browserInteraction.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormId), TimeSpan.FromSeconds(2));

            if (formIdElement == null)
                CommandResult.Fail(true, Constants.ErrorCodes.FORMID_NOT_FOUND);

            var route = formIdElement.GetAttribute("route");
            Logger.WriteLine($"Determining form: {route}");

            var formId = Guid.Parse(route.Substring(route.LastIndexOf('/') + 1));
            return CommandResult<SystemForm>.Success(SystemForm.GetById(GlobalTestingContext.ConnectionManager.AdminConnection, formId));
        }
    }
}