using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.EasyRepro.Web
{
    class WebBrowserEntity : BaseBrowserEntity
    {
        private readonly Browser _webBrowser;
        private readonly ButtonTexts _buttonTexts;
        
        public WebBrowserEntity(WebBrowser browser, Browser webBrowser, ButtonTexts buttonTexts)
            : base(browser)
        {
            _webBrowser = webBrowser;
            _buttonTexts = buttonTexts;
        }

        public override void DeleteRecord()
        {
            _webBrowser.Entity.ClickCommand(_buttonTexts.Delete);
            _webBrowser.Dialogs.Delete();
        }

        public override Guid GetId()
        {
            return Guid.Parse(_webBrowser.Driver.ExecuteScript("return Xrm.Page.data.entity.getId()").ToString());
        }

        public override bool IsFieldVisible(string fieldLogicalName)
        {
            return _webBrowser.Entity.Execute($"Is Field visible: {fieldLogicalName}", driver =>
            {
                return driver.WaitUntilVisible(By.Id(fieldLogicalName), TimeSpan.FromSeconds(5));
            });
        }

        public override void SaveRecord(bool saveIfDuplicate)
        {
            _webBrowser.CommandBar.ClickCommand(_buttonTexts.Save);
            // ThinkTime before call to DuplicateDetection, frame must be visible before the method call in order to succeed
            _webBrowser.ThinkTime(2000);
            _webBrowser.Dialogs.DuplicateDetection(saveIfDuplicate);
            _webBrowser.ThinkTime(2000);
            _webBrowser.Entity.SwitchToContentFrame();
        }

        protected override IFormFiller CreateBrowserFiller()
        {
            return new WebFormFiller(_webBrowser);
        }

        protected override IWebDriver GetWebDriver()
        {
            return _webBrowser.Driver;
        }

        protected override void SelectTab(string tabName)
        {
            _webBrowser.Entity.SelectTab(tabName);
        }
    }
}
