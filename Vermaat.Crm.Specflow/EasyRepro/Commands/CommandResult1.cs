namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class CommandResult<T>
    {
        private CommandResult()
        {
            AllowRetry = true;
        }

        public bool AllowRetry { get; set; }
       
        public int ErrorCode { get; set; }

        public object[] ErrorMessageFormatArgs { get; set; }

        public bool IsSuccessfull { get; set; }

        public T Result { get; set; }

        public static CommandResult<T> Success(T result)
           => new CommandResult<T>()
           {
               IsSuccessfull = true,
               Result = result,
           };

        public static CommandResult<T> Fail(bool allowRetry, int errorCode, params object[] formatArgs)
            => new CommandResult<T>()
            {
                AllowRetry = allowRetry,
                ErrorCode = errorCode,
                ErrorMessageFormatArgs = formatArgs,
                IsSuccessfull = false,
            };
    }
}