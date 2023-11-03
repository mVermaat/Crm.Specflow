using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vermaat.Crm.Specflow.Commands
{
    class AssertErrorDialogCommand : BrowserOnlyCommand
    {
        private readonly string _expectedError;

        public AssertErrorDialogCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, string expectedError)
            : base(crmContext, seleniumContext)
        {
            _expectedError = expectedError;
        }

        public override void Execute()
        {
            var formData = _seleniumContext.GetBrowser().LastFormData;
            Assert.AreEqual(_expectedError, formData.GetErrorDialogMessage());
        }
    }
}
