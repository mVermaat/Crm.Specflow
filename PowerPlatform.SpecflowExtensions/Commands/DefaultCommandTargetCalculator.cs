using PowerPlatform.SpecflowExtensions.Interfaces;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public sealed class DefaultCommandTargetCalculator : ICommandTargetCalculator
    {
        public CommandTarget Calculate(ICrmContext crmContext, ICommand command)
        {
            return Calculate(crmContext);
        }

        public CommandTarget Calculate<TResult>(ICrmContext crmContext, ICommandFunc<TResult> command)
        {
            return Calculate(crmContext);
        }

        private CommandTarget Calculate(ICrmContext crmContext)
        {
            if (crmContext.IsTarget(Constants.SpecFlow.TARGET_API))
            {
                return CommandTarget.PreferApi;
            }
            else if (crmContext.IsTarget(Constants.SpecFlow.TARGET_Chrome) ||
                     crmContext.IsTarget(Constants.SpecFlow.TARGET_Edge) ||
                     crmContext.IsTarget(Constants.SpecFlow.TARGET_Firefox) ||
                     crmContext.IsTarget(Constants.SpecFlow.TARGET_InternetExplorer))
            {
                return CommandTarget.PreferBrowser;
            }
            else
            {
                throw new TestExecutionException(Constants.ErrorCodes.UNKNOWN_TAG);
            }
        }
    }
}
