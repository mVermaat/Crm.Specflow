using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormSubgrid : FormComponent
    {
        public FormSection Section { get; set; }

        public FormSubgrid(UCIApp app) : base(app)
        {
        }

        public override string GetTabLabel()
        {
            return Section.Tab.Label;
        }

        public override string GetTabName()
        {
            return Section.Tab.Name;
        }

        public override bool IsVisible()
        {
            var by = By.XPath(AppElements.Xpath[AppReference.Entity.SubGridTitle].Replace("[NAME]", Label));
            var elements = App.WebDriver.FindElements(by);

            return elements.Count != 0;
        }
    }
}
