using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class SeleniumCommandFactory
    {
        public virtual CheckFieldVisibilityCommand CreateCheckFieldVisibilityCommand(string controlName)
            => new CheckFieldVisibilityCommand(controlName);

        public virtual ClickRibbonItemCommand CreateClickRibbonItemCommand(string name)
            => new ClickRibbonItemCommand(name);

        public virtual ExpandBusinessProcessStageCommand CreateExpandBusinessProcessStageCommand(string stageName)
            => new ExpandBusinessProcessStageCommand(stageName);

        public virtual GetAccessForUserCommand CreateGetAccessForUserCommand()
            => new GetAccessForUserCommand();

        public virtual GetBusinessProcessFlowDefinitionCommand CreateGetGetBusinessProcessFlowDefinitionCommand()
            => new GetBusinessProcessFlowDefinitionCommand();

        public virtual GetCompositeControlFieldsCommand CreateGetCompositeControlFieldsCommand(string compositeControlAttributeName)
           => new GetCompositeControlFieldsCommand(compositeControlAttributeName);

        internal virtual GetCurrentFormCommand CreateGetCurrentFormCommand(bool isQuickCreate)
            => new GetCurrentFormCommand(isQuickCreate);

        public virtual GetErrorDialogMessageCommand CreateGetErrorDialogMessageCommand(bool dialogMandatory)
            => new GetErrorDialogMessageCommand(dialogMandatory);

        public virtual GetFormNotificationsCommand CreateGetFormNotificationsCommand()
            => new GetFormNotificationsCommand();

        public virtual GetRibbonItemCommand CreateGetRibbonItemCommand(string name)
            => new GetRibbonItemCommand(name);

        public virtual OpenRecordCommand CreateOpenRecordCommand(OpenFormOptions formOptions, Guid? currentAppId)
            => new OpenRecordCommand(formOptions, currentAppId);

        public virtual SaveRecordCommand CreateSaveRecordCommand(bool saveIfDuplicate)
            => new SaveRecordCommand(saveIfDuplicate);

        public virtual SetBusinessProcessFlowBooleanFieldValueCommand CreateSetBusinessProcessFlowBooleanFieldValueCommand(BooleanItem item)
            => new SetBusinessProcessFlowBooleanFieldValueCommand(item);

        public virtual SetBusinessProcessFlowDateTimeFieldValueCommand CreateSetBusinessProcessFlowDateTimeFieldValueCommand(
            string logicalName, DateTime? value, bool dateOnly, string formatDate, string formatTime)
            => new SetBusinessProcessFlowDateTimeFieldValueCommand(logicalName, value, dateOnly, formatDate, formatTime);

        public virtual SetBusinessProcessFlowTextFieldValueCommand CreateSetBusinessProcessFlowTextFieldValueCommand(string logicalName, string value)
            => new SetBusinessProcessFlowTextFieldValueCommand(logicalName, value);

        public virtual SetDateTimeFieldValueCommand CreateSetDateTimeFieldValueCommand(
            string logicalName, DateTime? value, bool dateOnly, string formatDate, string formatTime)
            => new SetDateTimeFieldValueCommand(logicalName, value, dateOnly, formatDate, formatTime);
    }
}
