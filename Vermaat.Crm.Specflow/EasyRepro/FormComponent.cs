namespace Vermaat.Crm.Specflow.EasyRepro
{
    public abstract class FormComponent
    {
        public string Name { get; set; }
        public string Label { get; set; }

        protected UCIApp App { get; }

        protected FormComponent(UCIApp app)
        {
            App = app;
        }

        public abstract bool IsVisible();
        public abstract string GetTabName();
        public abstract string GetTabLabel();
    }
}
