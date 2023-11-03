namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class LongValue
    {
        public LongValue(long? value)
        {
            Value = value;
        }

        public long? Value { get; }

        public string TextValue => Value?.ToString(
            GlobalTestingContext.ConnectionManager.CurrentConnection.UserSettings.NumberFormat);
    }
}
