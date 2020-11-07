﻿using BoDi;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using PowerPlatform.SpecflowExtensions.EasyRepro;
using PowerPlatform.SpecflowExtensions.EasyRepro.Apps;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium.Entity;
using PowerPlatform.SpecflowExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class CloseOpportunityCommand : BrowserCommand
    {
        private readonly string _alias;
        private readonly Table _closeData;

        public CloseOpportunityCommand(IObjectContainer container,
            string alias, Table closeData) : base(container)
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

            GlobalContext.ConnectionManager.CurrentCrmService.Execute<OrganizationResponse>(request);
        }

        protected override void ExecuteBrowser()
        {
            var record = _crmContext.RecordCache.Get(_alias, true);
            var close = OpportunityCloseHelper.Create(_crmContext, _closeData, record);

            var session = GlobalContext.ConnectionManager.GetCurrentBrowserSession(_seleniumContext);
            var app = session.GetApp<CustomerEngagementApp>(_container);
            app.OpenRecord(new OpenFormOptions(record));

            var closeApp = session.GetApp<OpportunityActions>(_container);
            closeApp.CloseOpportunity(_crmContext, app.Navigation, close.Win, _closeData);

        }


        private class OpportunityCloseHelper
        {
            public Entity Entity { get; set; }
            public string StatusReasonText { get; set; }
            public int StatusReasonNumber { get; set; }
            public bool Win { get; set; }

            public static OpportunityCloseHelper Create(ICrmContext crmContext, Table table, EntityReference opportunity)
            {
                var result = new OpportunityCloseHelper
                {
                    Entity = new Entity("opportunityclose")
                };
                result.Entity.Attributes["opportunityid"] = opportunity;
                foreach (var row in table.Rows)
                {
                    if (row[Constants.SpecFlow.TABLE_KEY] == "opportunitystatuscode")
                    {
                        result.StatusReasonText = row[Constants.SpecFlow.TABLE_VALUE];
                        var statusData = ObjectConverter.FromStatusText(opportunity, result.StatusReasonText, crmContext);
                        result.StatusReasonNumber = statusData.StatusCode;
                        result.Win = statusData.StateCode == 1;
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