namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class DoubleValue
    {
        public DoubleValue(double? value)
        {
            Value = value;
        }

        public double? Value { get; }

        public string TextValue => Value?.ToString(
            GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.NumberFormat);
    }
}
