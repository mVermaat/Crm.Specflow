using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    internal class Navigation : INavigation
    {
        private readonly SeleniumExecutor _executor;

        public Navigation(SeleniumExecutor executor)
        {
            _executor = executor;
        }

        public void OpenRecord(OpenFormOptions options, Guid appId)
        {
            Logger.WriteLine($"Opening record {options.EntityName} with ID {options.EntityId}");
            _executor.Execute($"Open: {options.EntityName}", (driver, selectors) =>
            {
                driver.Navigate().GoToUrl(options.GetUrl(appId));
                CheckAlert(driver);
                WaitForFormLoad();

                if (driver.HasElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_ScriptErrorDialog)))
                    throw new TestExecutionException(Constants.ErrorCodes.FORMLOAD_SCRIPT_ERROR_ON_FORM);

                return true;
            });
        }

        public void WaitForFormLoad(params IFormLoadCondition[] additionalConditions)
        {
            _executor.Execute("Wait for form load", (driver, selectors) =>
            {
                DateTime timeout = DateTime.Now.AddSeconds(30);

                bool loadComplete = false;
                while (!loadComplete)
                {
                    loadComplete = true;

                    TimeSpan timeLeft = timeout.Subtract(DateTime.Now);
                    if (timeLeft.TotalMilliseconds > 0)
                    {
                        driver.WaitForPageToLoad();
                        driver.WaitUntilClickable(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormLoad),
                            timeLeft,
                            null,
                            () => { throw new TestExecutionException(Constants.ErrorCodes.FORM_LOAD_TIMEOUT); }
                        );

                        if (additionalConditions != null)
                        {
                            foreach (var condition in additionalConditions)
                            {
                                if (!condition.Evaluate(driver))
                                {
                                    Logger.WriteLine("Evaluation failed. Waiting for next attempt");
                                    loadComplete = false;
                                    Thread.Sleep(100);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new TestExecutionException(Constants.ErrorCodes.FORM_LOAD_TIMEOUT);
                    }
                }
                Logger.WriteLine("Form load completed");
                return true;
            });
        }

        private void CheckAlert(IWebDriver driver)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch (NoAlertPresentException)
            {
            }
        }
    }
}
