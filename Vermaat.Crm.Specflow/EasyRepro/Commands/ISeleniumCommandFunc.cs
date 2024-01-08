namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public interface ISeleniumCommandFunc<T>
    {
        CommandResult<T> Execute(BrowserInteraction browserInteraction);
    }
}
