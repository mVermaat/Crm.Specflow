using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Selenium
{
    public class SeleniumContext
    {
        public RemoteWebDriver WebDriver { get; set; }

        public string BaseUrl { get; set; }


        public static SeleniumContext CreateContext()
        {
            return new SeleniumContext()
            {
                BaseUrl = ConfigurationManager.AppSettings["Url"],
                WebDriver = GetWebDriver()
            };
        }

        private static RemoteWebDriver GetWebDriver()
        {
            var runner = ConfigurationManager.AppSettings["Runner"];

            switch(runner.ToLower())
            {
                case "chrome": return new ChromeDriver();
                default: throw new ArgumentException(string.Format("Runner {0} not supported", runner));
            }
        }
    }
}
