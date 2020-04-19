namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormSection : FormComponent
    {
        public FormTab Tab { get; set; }

        public FormSection(UCIApp app) : base(app)
        {
        }

        public override bool IsVisible()
        {
            var by = SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Section, Label);
            var elements = App.WebDriver.FindElements(by);

            return elements.Count != 0;
        }

        public override string GetTabName()
        {
            return Tab.Name;
        }

        public override string GetTabLabel()
        {
            return Tab.Label;
        }
    }
}
