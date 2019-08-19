using Microsoft.Xrm.Sdk;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class OpenFormOptions
    {
        public OpenFormOptions(EntityReference recordToOpen)
        {
            EntityName = recordToOpen.LogicalName;
            EntityId = recordToOpen.Id;
        }

        public OpenFormOptions(string entityName)
        {
            EntityName = entityName;
        }


        public EntityReference Parent { get; set; }
        public string EntityName { get; }
        public Guid? EntityId { get; }
        public Guid? FormId { get; set; }

        public string GetUrl(IWebDriver driver, Guid? appId)
        {
            Uri currentUrl = new Uri(driver.Url);

            StringBuilder builder = new StringBuilder($"{currentUrl.Scheme}://{currentUrl.Authority}/main.aspx?etn={EntityName}&pagetype=entityrecord&flags=testmode=true");

            if (EntityId.HasValue)
                builder.Append($"&id=%7B{EntityId:D}%7D");
            if (appId.HasValue)
                builder.Append($"&appid={appId:D}");

            StringBuilder extraQueryParameters = new StringBuilder();
            if(Parent != null)
            {
                AppendQueryParameter(extraQueryParameters, "parentrecordid", Parent.Id);
                AppendQueryParameter(extraQueryParameters, "parentrecordname", Parent.Name);
                AppendQueryParameter(extraQueryParameters, "parentrecordtype", Parent.LogicalName);
            }
            if(FormId != null)
            {
                AppendQueryParameter(extraQueryParameters, "formid", FormId);
            }
            if(extraQueryParameters.Length > 0)
            {
                builder.Append("&extraqs=");
                builder.Append(HttpUtility.UrlEncode(extraQueryParameters.ToString()));
            }
            return builder.ToString();
        }

        private void AppendQueryParameter(StringBuilder builder, string key, object value)
        {
            if(builder.Length > 0)
            {
                builder.Append("&");
            }

            builder.Append(key);
            builder.Append("=");
            builder.Append(value);
        }
    }
}
