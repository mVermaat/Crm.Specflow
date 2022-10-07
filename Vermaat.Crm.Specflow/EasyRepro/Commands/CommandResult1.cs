namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class CommandResult<T>
    {
        public CommandResult()
        {
            AllowRetry = true;
        }

        public bool AllowRetry { get; set; }
        public bool IsSuccessfull { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}