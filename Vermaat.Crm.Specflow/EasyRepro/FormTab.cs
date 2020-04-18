namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormTab : FormComponent
    {
        public string Label { get; set; }
        public bool Visible { get; set; }

        public override bool IsVisible()
        {
            return Visible;
        }
    }
}
