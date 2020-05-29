using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Fields
{
    public class BodyFormField : FormField
    {
        private string _tabLabel;

        public BodyFormField(UCIApp app, AttributeMetadata attributeMetadata, string control) 
            : base(app, attributeMetadata, control)
        {
        }

        public override bool IsVisible(FormState formState)
        {
            formState.ExpandTab(GetTabLabel());
            return base.IsVisible(formState);
        }

        public override RequiredState GetRequiredState(FormState formState)
        {
            formState.ExpandTab(GetTabLabel());
            return base.GetRequiredState(formState);
        }

        public override bool IsLocked(FormState formState)
        {
            formState.ExpandTab(GetTabLabel());
            return base.IsLocked(formState);
        }

        private string GetTabLabel()
        {
            if (string.IsNullOrEmpty(_tabLabel))
            {
                _tabLabel = App.WebDriver.ExecuteScript($"return Xrm.Page.getControl('{Control}').getParent().getParent().getLabel()")?.ToString();
            }
            return _tabLabel;
        }
    }
}
