using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro.Controls
{
    internal abstract class Control
    {
        public string ControlName { get; private set; }

        public Control(string control)
        {
            ControlName = control;
        }
    }
}
