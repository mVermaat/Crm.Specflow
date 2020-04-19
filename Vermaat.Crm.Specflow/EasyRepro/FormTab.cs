using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormTab : FormComponent
    {
        public string Label { get; set; }
        public Dictionary<string, FormSection> Sections { get; private set; }

        public FormTab(UCIApp app, Dictionary<string, FormSection> sections) : base(app)
        {
            Sections = sections;
        }

        public override bool IsVisible()
        {
            var by = By.XPath(string.Format(AppElements.Xpath[AppReference.Entity.Tab], Label));
            var elements = App.WebDriver.FindElements(by);

            return elements.Count != 0;
        }
    }
}
