using PowerPlatform.SpecflowExtensions.Interfaces;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public interface ICommand
    {
        void Execute(ICommandTargetCalculator targetCalculator, CommandTarget? prefferedTarget);
    }
}
