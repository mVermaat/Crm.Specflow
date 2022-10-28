using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class SeleniumCommandFactory
    {
        public virtual ClickRibbonItemCommand CreateClickRibbonItemCommand(string name)
            => new ClickRibbonItemCommand(name);

        public virtual GetFormNotificationsCommand CreateGetFormNotificationsCommand()
            => new GetFormNotificationsCommand();

        public virtual GetRibbonItemCommand CreateGetRibbonItemCommand(string name)
            => new GetRibbonItemCommand(name);

        public virtual SaveRecordCommand CreateSaveRecordCommand(bool saveIfDuplicate)
            => new SaveRecordCommand(saveIfDuplicate);
    }
}
