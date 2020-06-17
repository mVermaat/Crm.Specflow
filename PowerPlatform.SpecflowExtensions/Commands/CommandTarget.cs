using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.Commands
{
    public enum CommandTarget
    {
        ForceApi = 1,
        ForceBrowser = 2,
        PreferApi = 3,
        PreferBrowser = 4
    }
}
