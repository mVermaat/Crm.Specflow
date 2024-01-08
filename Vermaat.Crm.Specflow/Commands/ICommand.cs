namespace Vermaat.Crm.Specflow.Commands
{
    public interface ICommand
    {
        void Execute(CommandAction commandAction);
    }
}
