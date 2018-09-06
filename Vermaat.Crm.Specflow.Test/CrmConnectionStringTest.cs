using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vermaat.Crm.Specflow.Test
{
    [TestClass]
    public class CrmConnectionStringTest
    {
        [TestMethod]
        public void UsernamePasswordOptional()
        {
            var crmConnection = CrmConnectionString.CreateFromAppConfig();
            var result = crmConnection.ToCrmClientString();

            Assert.AreEqual("AuthType=AD;Url=http://thisdoesntexist.testing", result);
        }

        [TestMethod]
        public void FullConnectionStringTest()
        {
            var crmConnection = new CrmConnectionString
            {
                AuthType = "AD",
                Password = "SomePassword",
                Url = "http://someurl.com",
                Username = "MyUser"
            };

            var result = crmConnection.ToCrmClientString();

            Assert.AreEqual("AuthType=AD;Url=http://someurl.com;Username=MyUser;Password=SomePassword", result);
        }
    }
}
