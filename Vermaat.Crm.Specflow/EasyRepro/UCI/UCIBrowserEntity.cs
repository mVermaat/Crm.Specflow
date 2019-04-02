using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using TechTalk.SpecFlow;

namespace Vermaat.Crm.Specflow.EasyRepro.UCI
{
    class UCIBrowserEntity : BaseBrowserEntity
    {
        private readonly WebClient _client;
        private readonly XrmApp _app;
        private readonly ButtonTexts _buttonTexts;

        public UCIBrowserEntity(UCIBrowser browser, WebClient client, XrmApp app, ButtonTexts buttonTexts) :
            base(browser)
        {
            _client = client;
            _app = app;
            _buttonTexts = buttonTexts;
        }

        public override void DeleteRecord()
        {
            _app.CommandBar.ClickCommand(_buttonTexts.Delete);
            _app.Dialogs.ConfirmationDialog(true);
        }

        public override Guid GetId()
        {
            return _app.Entity.GetObjectId();
        }

        public override bool IsFieldVisible(string fieldLogicalName)
        {
            if (!IsFieldOnForm(fieldLogicalName))
                return false;

            if (!IsTabOfFieldExpanded(fieldLogicalName))
                ExpandTabThatContainsField(fieldLogicalName);

            return _client.Execute($"Is Field visible: {fieldLogicalName}", driver =>
            {
                return driver.WaitUntilVisible(By.XPath(
                    AppElements.Xpath[AppReference.Entity.TextFieldContainer].Replace("[NAME]", fieldLogicalName)), TimeSpan.FromSeconds(5));
            });
        }

        public override void SaveRecord(bool saveIfDuplicate)
        {
            _app.Entity.Save();
            ConfirmDuplicate(saveIfDuplicate);
            WaitUntilSaveCompleted();
        }

        private void WaitUntilSaveCompleted()
        {
            _client.Execute($"Waiting until save is completed", driver =>
            {
                var timeout = DateTime.Now.AddSeconds(20);
                bool saveCompleted = false;
                while (!saveCompleted && DateTime.Now < timeout)
                {
                    var footerElement = driver.FindElement(By.XPath("//span[@data-id='edit-form-footer-message']"));

                    if (string.IsNullOrEmpty(footerElement.Text) || footerElement.Text.ToLower() != "saving")
                        return true;
                    else
                        Thread.Sleep(2500);
                }

                return false;
            });
        }

        private void ConfirmDuplicate(bool saveIfDuplicate)
        {
            _client.Execute($"Checking for duplicate detection. Ignore: {saveIfDuplicate}", driver =>
            {
                driver.WaitUntilAvailable(By.XPath("//div[contains(@id,'dialogFooterContainer_')]"), new TimeSpan(0, 0, 5),
                    d =>
                    {
                        if(saveIfDuplicate)
                        {
                            d.ClickWhenAvailable(By.Id("id-125fc733-aabe-4bd2-807e-fd7b6da72779-4"));
                        }
                        else
                        {
                            throw new ArgumentException("Duplicate found and not selected for save");
                        }
                    });
                return true;
            });
        }

        protected override IFormFiller CreateBrowserFiller()
        {
            return new UCIFormFiller(_app.Entity, _client);
        }

        protected override IWebDriver GetWebDriver()
        {
            return _client.Browser.Driver;
        }

        protected override void SelectTab(string tabName)
        {
            _app.Entity.SelectTab(tabName);
        }
    }
}
