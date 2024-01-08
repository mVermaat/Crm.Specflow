namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public interface ISeleniumCommand
    {
        CommandResult Execute(BrowserInteraction browserInteraction);
    }
}
