using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public interface ISeleniumCommandFunc<T>
    {
        CommandResult<T> Execute(BrowserInteraction browserInteraction);
    }
}
