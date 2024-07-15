using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;

namespace Vermaat.Crm.Specflow.EasyRepro.Commands
{
    public class SeleniumCommandFactory
    {
        public virtual CheckFieldLockedStateCommand CreateCheckFieldLockedStateCommand(string controlName)
            => new CheckFieldLockedStateCommand(controlName);

        public virtual CheckFieldRequiredStateCommand CreateCheckFieldRequiredStateCommand(string logicalName)
            => new CheckFieldRequiredStateCommand(logicalName);

        public virtual CheckFieldVisibilityCommand CreateCheckFieldVisibilityCommand(string controlName)
            => new CheckFieldVisibilityCommand(controlName);

        public virtual CheckForDuplicateDetection CreateCheckForDuplicateDetection(bool saveIfDuplicate)
            => new CheckForDuplicateDetection(saveIfDuplicate);

        public virtual ClearLookupValueCommand CreateClearLookupValueCommand(LookupItem lookupItem)
            => new ClearLookupValueCommand(lookupItem); 

        public virtual ClickRibbonItemCommand CreateClickRibbonItemCommand(string name)
            => new ClickRibbonItemCommand(name);

        public virtual ConvertActiveQuoteToSalesOrderCommand CreateConvertActiveQuoteToSalesOrderCommand()
            => new ConvertActiveQuoteToSalesOrderCommand();

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

        public virtual GetCurrentRecordIdCommand CreateGetCurrentRecordIdCommand()
            => new GetCurrentRecordIdCommand();

        public virtual GetErrorDialogMessageCommand CreateGetErrorDialogMessageCommand(bool dialogMandatory)
            => new GetErrorDialogMessageCommand(dialogMandatory);

        public virtual GetFormNotificationsCommand CreateGetFormNotificationsCommand()
            => new GetFormNotificationsCommand();

        public virtual GetRibbonItemCommand CreateGetRibbonItemCommand(string name, string buttonDelimiter = ".")
            => new GetRibbonItemCommand(name, buttonDelimiter);

        public virtual OpenQuickCreatedRecordCommand CreateOpenQuickCreatedRecordCommand(string childEntityName)
            => new OpenQuickCreatedRecordCommand(childEntityName);

        public virtual OpenRecordCommand CreateOpenRecordCommand(OpenFormOptions formOptions, Guid? currentAppId)
            => new OpenRecordCommand(formOptions, currentAppId);

        public virtual ReviseQuoteCommand CreateReviseQuoteCommand()
            => new ReviseQuoteCommand();

        public virtual SaveQuickCreateRecord CreateSaveQuickCreateRecord(bool saveIfDuplicate)
            => new SaveQuickCreateRecord(saveIfDuplicate);

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
