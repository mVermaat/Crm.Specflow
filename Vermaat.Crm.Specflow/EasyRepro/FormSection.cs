using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormSection : FormComponent
    {
        public string Label { get; set; }
        public FormTab Tab { get; set; }

        public FormData _formData;

        public FormSection(FormData formData, UCIApp app) : base(app)
        {
            _formData = formData;
        }

        public override bool IsVisible()
        {
            _formData.ExpandTab(Tab.Label);

            var by = SeleniumFunctions.Selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_Section, Label);
            var elements = App.WebDriver.FindElements(by);

            return elements.Count != 0;
        }
    }
}
