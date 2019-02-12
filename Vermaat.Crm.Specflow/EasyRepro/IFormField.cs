using Microsoft.Dynamics365.UIAutomation.Api;
using OpenQA.Selenium;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    interface IFormField
    {
        string FieldName { get; }

        bool IsOnForm(IWebDriver driver);
        void EnterOnForm(Entity entity);
    }
}