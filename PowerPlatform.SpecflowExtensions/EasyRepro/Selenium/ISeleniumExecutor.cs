using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using OpenQA.Selenium;
using System;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Selenium
{
    public interface ISeleniumExecutor
    {
        TResult Execute<TResult>(string commmandName, Func<IWebDriver, SeleniumSelectorData, TResult> func);
        TResult Execute<TResult>(string commmandName, Func<IWebDriver, SeleniumSelectorData, XrmApp, TResult> func);
        TResult Execute<TResult>(string commmandName, Func<IWebDriver, TResult> func);
    }
}