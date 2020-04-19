using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public abstract class FormComponent
    {
        protected UCIApp App { get; }

        public FormComponent(UCIApp app)
        {
            App = app;
        }

        public abstract bool IsVisible();
    }
}
