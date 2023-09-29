using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class GetCurrentRecordIdCommand : ISeleniumCommandFunc<Guid>
    {
        public CommandResult<Guid> Execute(BrowserInteraction browserInteraction)
        {
            Logger.WriteLine("Getting Record Id");
            var queryParams = HttpUtility.ParseQueryString(new Uri(browserInteraction.Driver.Url).Query);

            if (Guid.TryParse(queryParams["id"], out var id))
            {
                Logger.WriteLine($"Found current record id via url: {id}");
                return CommandResult<Guid>.Success(id);
            }

            var objectId = browserInteraction.Driver.ExecuteScript("return Xrm.Page.data.entity.getId();");

            if (Guid.TryParse(objectId.ToString(), out id))
            {
                Logger.WriteLine($"Found current record id via script: {id}");
                return CommandResult<Guid>.Success(id);
            }

            return CommandResult<Guid>.Fail(true, Constants.ErrorCodes.ENTITY_ID_NOT_FOUND);

        }
    }
}
