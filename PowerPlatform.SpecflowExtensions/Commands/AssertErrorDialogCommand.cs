using BoDi;
using FluentAssertions;
using PowerPlatform.SpecflowExtensions.EasyRepro.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public class AssertErrorDialogCommand : BrowserOnlyCommand
    {
        private readonly string _expectedError;

        public AssertErrorDialogCommand(IObjectContainer container, string expectedError)
            : base(container)
        {
            _expectedError = expectedError;
        }

        public override void Execute()
        {
            var form = GlobalContext.ConnectionManager
               .GetCurrentBrowserSession(_seleniumContext)
               .GetApp<CustomerEngagementApp>(_container)
               .LastForm;

            form.GetErrorDialog().Message.Should().Be(_expectedError);
        }
    }
}
