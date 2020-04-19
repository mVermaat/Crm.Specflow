using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormTab : FormComponent
    {
        public FormComponentCollection<FormSection> Sections { get; private set; }

        public FormTab(UCIApp app, FormComponentCollection<FormSection> sections) : base(app)
        {
            Sections = sections;
        }

        public override bool IsVisible()
        {
            var by = By.XPath(string.Format(AppElements.Xpath[AppReference.Entity.Tab], Label));
            var elements = App.WebDriver.FindElements(by);

            return elements.Count != 0;
        }

        public override string GetTabName()
        {
            return Name;
        }

        public override string GetTabLabel()
        {
            return Label;
        }
    }
}
