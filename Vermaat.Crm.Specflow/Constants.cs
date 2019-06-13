using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public static class Constants
    {
        public static class SpecFlow
        {
            public const string TABLE_KEY = "Property";
            public const string TABLE_VALUE = "Value";
            public const string TABLE_FORMSTATE = "State";

            public const string TARGET_API = "API";
            public const string TARGET_Chrome = "Chrome";
            public const string TARGET_Firefox = "Firefox";
            public const string TARGET_InternetExplorer = "IE";
            public const string TARGET_Edge = "Edge";
        }

        public class CRM
        {
            public const string STATUSCODE = "statuscode";
        }

        public class XPath
        {
            public const string FIELD_ISLOCKED = "//label[contains(@id, '[NAME]-locked-icon')]";
            public const string FIELD_ISREQUIREDORRECOMMEND = "//div[contains(@id, '[NAME]-required-icon')]";
            public const string DIALOG_CONTAINER = "id(\"dialogContentContainer_1\")";
            public const string DIALOG_OK = "//button[@data-id='ok_id']";
        }
    }
}
