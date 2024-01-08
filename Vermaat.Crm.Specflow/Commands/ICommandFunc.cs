namespace Vermaat.Crm.Specflow.Commands
{
    public interface ICommandFunc<TResult>
    {
        TResult Execute(CommandAction commandAction);
    }
}
