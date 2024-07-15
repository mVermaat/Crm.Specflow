using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class CheckForDuplicateDetection : ISeleniumCommandFunc<DuplicateDetectionResult>
    {
        private readonly bool _saveIfDuplicate;

        public CheckForDuplicateDetection(bool saveIfDuplicate)
        {
            _saveIfDuplicate = saveIfDuplicate;
        }

        public CommandResult<DuplicateDetectionResult> Execute(BrowserInteraction browserInteraction)
        {
            if(HasDuplicateDetection(browserInteraction))
            {
                if(_saveIfDuplicate)
                {
                    AcceptDuplicateDetection(browserInteraction.Driver);
                    Logger.WriteLine("Accepted duplicate");
                    return CommandResult<DuplicateDetectionResult>.Success(DuplicateDetectionResult.DuplicateDetectionAccepted);
                }
                else
                {
                    Logger.WriteLine("Duplicate detection rejected");
                    return CommandResult<DuplicateDetectionResult>.Success(DuplicateDetectionResult.DuplicateDetectionRejected);
                }
            }
            else
            {
                Logger.WriteLine("No duplicate detection");
                return CommandResult<DuplicateDetectionResult>.Success(DuplicateDetectionResult.NoDuplicateDetection);
            }
        }

        private bool HasDuplicateDetection(BrowserInteraction browserInteraction)
        {
            return browserInteraction.Driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionWindowMarker]))
                && browserInteraction.Driver.HasElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows]));
        }

        private void AcceptDuplicateDetection(IWebDriver driver)
        {
            Logger.WriteLine("Accepting duplicate");
            //Select the first record in the grid
            driver.FindElements(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionGridRows]))[0].Click(true);

            //Click Ignore and Save
            driver.FindElement(By.XPath(AppElements.Xpath[AppReference.Entity.DuplicateDetectionIgnoreAndSaveButton])).Click(true);
            driver.WaitForTransaction();
        }
    }
}
