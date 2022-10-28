using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public class ErrorCodes
    {
        private readonly Dictionary<int, string> _errorMessages;

        public ErrorCodes()
        {
            _errorMessages = new Dictionary<int, string>();
            FillDictionary();
        }

        private void FillDictionary()
        {
            AddError(Constants.ErrorCodes.ERRORMESSAGE_NOT_FOUND, "Errormessage with ErrorCode {0} not found");
            AddError(Constants.ErrorCodes.FORMLOAD_SCRIPT_ERROR_ON_FORM, "Failed to load form due to a JavaScript error");
            AddError(Constants.ErrorCodes.BROWSER_OT_SUPPORTED_FOR_API_TEST, "Browser not supported for API only commands");
            AddError(Constants.ErrorCodes.N_N_RELATIONSHIP_NOT_FOUND, "No N:N relationship found between {0} and {1}");
            AddError(Constants.ErrorCodes.RECORD_NOT_FOUND, "Cannot find record {0} of type {1}");
            AddError(Constants.ErrorCodes.UNKNOWN_TAG, "Unknown tag. Use API, Chrome, Edge, Firefox or IE");
            AddError(Constants.ErrorCodes.API_NOT_SUPPORTED_FOR_BROWSER_ONLY_COMMANDS, "API not supported for Browser only commands");
            AddError(Constants.ErrorCodes.CURRENT_BUSINESS_PROCESS_STAGE_NOT_FOUND, "Current stage can't be found");
            AddError(Constants.ErrorCodes.BUSINESS_PROCESS_STAGE_NOT_IN_ACTIVE_PATH, "{0} isn't in the active path");
            AddError(Constants.ErrorCodes.BUSINESS_PROCESS_STAGE_CANNOT_BE_LAST, "Current stage be the last");
            AddError(Constants.ErrorCodes.LANGUAGECODE_MUST_BE_INTEGER, "AppSettings languagecode must be an integer i.e. 1033 for english");
            AddError(Constants.ErrorCodes.FORM_SAVE_FAILED, "Save failed. {0}");
            AddError(Constants.ErrorCodes.FORM_SAVE_TIMEOUT, "Save wasn't completed in {0} seconds");
            AddError(Constants.ErrorCodes.DUPLICATE_RECORD_DETECTED, "Duplicate found and not selected for save");
            AddError(Constants.ErrorCodes.FIELD_NOT_ON_FORM, "Field {0} can't be found on form");
            AddError(Constants.ErrorCodes.TWO_OPTION_FIELDS_CANT_BE_CLEARED, "Two option fields can't be cleared");
            AddError(Constants.ErrorCodes.MORE_COMMANDS_NOT_FOUND, "More commands button not found");
            AddError(Constants.ErrorCodes.UNKNOWN_FORM_NOTIFICATION_TYPE, "Unknown notification type. Current class: {0}");
            AddError(Constants.ErrorCodes.INVALID_FORM_STATE, "Invalid value for form state: {0}");
            AddError(Constants.ErrorCodes.APP_SETTINGS_REQUIRED, "AppSetting {0} is required");
            AddError(Constants.ErrorCodes.LABEL_NOT_TRANSLATED, "Label {0} doesn't have a translation for language {1}");
            AddError(Constants.ErrorCodes.ATTRIBUTE_DOESNT_EXIST, "Attribute {0} not found for entity {1}");
            AddError(Constants.ErrorCodes.LOOKUP_NOT_FOUND, "Lookup named {0} was not found. Queried entities: {1}");
            AddError(Constants.ErrorCodes.OPTION_NOT_FOUND, "Field {0} doesn't have option {1}");
            AddError(Constants.ErrorCodes.CANT_START_BROWSER_FOR_API_TESTS, "Cannot start the browser if the target is API");
            AddError(Constants.ErrorCodes.FORM_LOAD_TIMEOUT, "CRM Record is Unavailable or not finished loading. Timeout Exceeded");
            AddError(Constants.ErrorCodes.UNABLE_TO_LOGIN, "Failed to login. Error: {0}");
            AddError(Constants.ErrorCodes.FORM_NOT_FOUND, "Form {0} of entity {1} wasn't found");
            AddError(Constants.ErrorCodes.APP_NOT_FOUND, "App {0} not found");
            AddError(Constants.ErrorCodes.INVALID_DATATYPE, "Invalid datatype for field {0}. Expected {1}");
            AddError(Constants.ErrorCodes.VALUE_NULL, "Field {0} requires a value, but it is empty");
            AddError(Constants.ErrorCodes.ELEMENT_NOT_INTERACTABLE, "Element not interactable. {0}");
            AddError(Constants.ErrorCodes.BUSINESS_PROCESS_ERROR_WHEN_LOADING, "Business Process Error: {0}");
            AddError(Constants.ErrorCodes.ASYNC_TIMEOUT, "Not all Asynchronous jobs are completed on time");
            AddError(Constants.ErrorCodes.MULTIPLE_ATTRIBUTES_FOUND, "Multiple attributes found for {0}. Results: {1}");
            AddError(Constants.ErrorCodes.APPLICATIONUSER_CANNOT_LOGIN, "An application user can't login via the browser");
            AddError(Constants.ErrorCodes.NO_CONTROL_FOUND, "No controls found for attribute {0}");
            AddError(Constants.ErrorCodes.RIBBON_NOT_FOUND, "Unable to find the {0} ribbon");
            AddError(Constants.ErrorCodes.UNEXPECTED_PROCESS_COUNT, "Expected 1 process with name {0}, but found {1}");
            AddError(Constants.ErrorCodes.USER_NOT_FOUND, "User {0} doesn't exist");
            AddError(Constants.ErrorCodes.QUICK_CREATE_CHILD_NOT_AVAILABLE, "Couldn't open quick created record");
            AddError(Constants.ErrorCodes.LOCALIZATION_OVERRIDES_NOT_FOUND, "Localization JSON file not found at: {0}");
            AddError(Constants.ErrorCodes.USERPROFILE_NOT_FOUND, "User profile '{0}' doesn't exist");
            AddError(Constants.ErrorCodes.USERPROFILE_FILE_NOT_FOUND, "User profile file '{0}' doesn't exist");
            AddError(Constants.ErrorCodes.TRYUNTIL_TIMEOUT, "Timeout for retry reached: {0}");
            AddError(Constants.ErrorCodes.SELECTOR_NOT_FOUND, "Selenium Selector {0} doesn't exist");
            AddError(Constants.ErrorCodes.LOCALIZED_LANGUAGE_MISSING, "No localized texts for language {0} are available. See https://github.com/DynamicHands/Crm.Specflow/wiki/Writing-Features#localization for more information.");
            AddError(Constants.ErrorCodes.SELENIUM_COMMAND_NO_RESULT, "Selenium command {0} didn't have a result. Can't display a proper error.");
            AddError(Constants.ErrorCodes.RIBBON_BUTTON_DOESNT_EXIT, "Ribbon button {0} doesn't exist");
            AddError(Constants.ErrorCodes.FAILED_TO_PARSE_CONDITION_OPERATOR, "Failed to parse condition '{0}'. {1}. See https://learn.microsoft.com/en-us/power-apps/developer/data-platform/webapi/reference/conditionoperator?view=dataverse-latest#members for all valid values");
        }

        public void AddError(int errorCode, string message)
        {
            _errorMessages.Add(errorCode, message);
        }

        public string GetErrorMessage(int errorCode, params object[] formatArgs)
        {
            return GetErrorMessage(errorCode, null, formatArgs);
        }

        public string GetErrorMessage(int errorCode, string additionalDetails, params object[] formatArgs)
        {
            Logger.WriteLine($"Getting error message for errorcode {errorCode}");
            if (!_errorMessages.ContainsKey(errorCode))
                return string.Format(_errorMessages[Constants.ErrorCodes.ERRORMESSAGE_NOT_FOUND], errorCode);

            return string.Format($"[{errorCode}] {_errorMessages[errorCode]}{additionalDetails}", formatArgs);
        }

        public void ChangeErrorMessage(int errorCode, string errorMessage)
        {
            _errorMessages[errorCode] = errorMessage;
        }
    }
}
