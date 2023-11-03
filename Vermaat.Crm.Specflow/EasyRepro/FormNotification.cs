namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormNotification
    {
        public FormNotificationType Type { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{Type}: {Message}";
        }
    }
}
