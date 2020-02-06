namespace Vermaat.Crm.Specflow
{
    public interface IConnectionStringHelper
    {
        string GetConnectionString();

        ValidationResult Validate();
    }
}