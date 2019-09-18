using Microsoft.Crm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro;
using Microsoft.Xrm.Sdk;
using Vermaat.Crm.Specflow.FormLoadConditions;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.Commands
{
    public class CloseOpportunityCommand : BrowserCommand
    {
        private readonly string _alias;
        private readonly Table _closeData;

        public CloseOpportunityCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext,
            string alias, Table closeData) : base(crmContext, seleniumContext)
        {
            _alias = alias;
            _closeData = closeData;
        }

        protected override void ExecuteApi()
        {
            var record = _crmContext.RecordCache.Get(_alias, true);
            var close = OpportunityCloseHelper.Create(_crmContext, _closeData, record);

            var request = close.Win ? new WinOpportunityRequest() as OrganizationRequest 
                                      : new LoseOpportunityRequest();
            request.Parameters["OpportunityClose"] = close.Entity;
            request.Parameters["Status"] = new OptionSetValue(close.StatusReasonNumber);

            GlobalTestingContext.ConnectionManager.CurrentConnection.Execute<OrganizationResponse>(request);
        }

        protected override void ExecuteBrowser()
        {
            var record = _crmContext.RecordCache.Get(_alias, true);
            var close = OpportunityCloseHelper.Create(_crmContext, _closeData, record);

            var browser = _seleniumContext.GetBrowser();
            browser.OpenRecord(new OpenFormOptions(record));
            var dialog = OpportunityCloseDialog.CreateDialog(browser.App, close.Win);
            dialog.EnterData(_crmContext, _closeData);
            dialog.FinishDialog();
        }


        private class OpportunityCloseHelper
        {
            public Entity Entity { get; set; }
            public string StatusReasonText { get; set; }
            public int StatusReasonNumber { get; set; }
            public bool Win { get; set; }

            public static OpportunityCloseHelper Create(CrmTestingContext crmContext, Table table, EntityReference opportunity)
            {
                var result = new OpportunityCloseHelper
                {
                    Entity = new Entity("opportunityclose")
                };
                result.Entity.Attributes["opportunityid"] = opportunity;
                foreach (var row in table.Rows)
                {
                    if(row[Constants.SpecFlow.TABLE_KEY] == "opportunitystatuscode")
                    {
                        result.StatusReasonText = row[Constants.SpecFlow.TABLE_VALUE];
                        var setStateRequest = ObjectConverter.ToSetStateRequest(opportunity, result.StatusReasonText, crmContext);
                        result.StatusReasonNumber = setStateRequest.Status.Value;
                        result.Win = setStateRequest.State.Value == 1;
                    }
                    else
                    {
                        result.Entity[row[Constants.SpecFlow.TABLE_KEY]] =
                            ObjectConverter.ToCrmObject(result.Entity.LogicalName, row[Constants.SpecFlow.TABLE_KEY],
                            row[Constants.SpecFlow.TABLE_VALUE], crmContext);
                    }
                }

                return result;
            }
        }
    }


}
