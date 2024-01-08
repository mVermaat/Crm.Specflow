using OpenQA.Selenium;

namespace Vermaat.Crm.Specflow.FormLoadConditions
{
    public interface IFormLoadCondition
    {
        bool Evaluate(IWebDriver driver);
    }
}
