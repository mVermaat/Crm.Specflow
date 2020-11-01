using BoDi;
using Microsoft.Xrm.Sdk;
using PowerPlatform.SpecflowExtensions.EasyRepro;
using PowerPlatform.SpecflowExtensions.EasyRepro.Apps;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System.Linq;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class UpdateRecordCommand : BrowserCommand
    {
        private static readonly string[] _apiOnlyEntities = new string[] { "usersettings" };

        private readonly EntityReference _toUpdate;
        private readonly Table _criteria;

        public UpdateRecordCommand(IObjectContainer container, EntityReference toUpdate,
            Table criteria)
            : base(container)
        {
            _toUpdate = toUpdate;
            _criteria = criteria;
        }

        protected override void ExecuteApi()
        {
            Entity toUpdate = new Entity(_toUpdate.LogicalName)
            {
                Id = _toUpdate.Id
            };

            foreach (TableRow row in _criteria.Rows)
            {
                toUpdate[row[Constants.SpecFlow.TABLE_KEY]] = ObjectConverter.ToCrmObject(_toUpdate.LogicalName,
                    row[Constants.SpecFlow.TABLE_KEY], row[Constants.SpecFlow.TABLE_VALUE], _crmContext);
            }

            GlobalContext.ConnectionManager.CurrentCrmService.Update(toUpdate);
        }

        protected override void ExecuteBrowser()
        {
            if (_apiOnlyEntities.Contains(_toUpdate.LogicalName))
            {
                ExecuteApi();
            }
            else
            {
                var formData = GlobalContext.ConnectionManager
                    .GetCurrentBrowserSession(_seleniumContext)
                    .GetApp<CustomerEngagementApp>(_container)
                    .OpenRecord(new OpenFormOptions(_toUpdate));
                formData.FillForm(_crmContext, _criteria);
                formData.Save(true);
            }
        }
    }
}
