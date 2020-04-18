using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow.Commands
{
    public class AssertFormStateCommand : BrowserOnlyCommand
    {
        private readonly EntityReference _crmRecord;
        private readonly Table _visibilityCriteria;

        public AssertFormStateCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, EntityReference crmRecord, Table visibilityCriteria) :
            base(crmContext, seleniumContext)
        {
            _crmRecord = crmRecord;
            _visibilityCriteria = visibilityCriteria;
        }

        public override void Execute()
        {
            var formData = _seleniumContext.GetBrowser().OpenRecord(new OpenFormOptions(_crmRecord));
            List<string> errors = new List<string>();

            string currentTab = null;
            foreach (TableRow row in _visibilityCriteria.Rows)
            {
                var expectedFormState = GetExpectedFormState(row[Constants.SpecFlow.TABLE_FORMSTATE]);
                var type = row.ContainsKey("Type") ? row["Type"] : "attribute";
                var key = row[Constants.SpecFlow.TABLE_KEY];

                // TODO: Move this to a separate function and throw exception if the type is not recognized
                var isOnForm =
                    (type == "attribute" && formData.ContainsField(key))
                    || (type == "tab" && formData.ContainsTab(key));

                if (isOnForm)
                {
                    var component = type == "attribute"
                        ? (FormComponent)formData.Fields[row[Constants.SpecFlow.TABLE_KEY]]
                        : formData.Tabs[row[Constants.SpecFlow.TABLE_KEY]];

                    var formField = component as FormField;
                    if (formField != null)
                    {
                        var newTab = formField.GetTabName();
                        // TODO: Use existing function for switching tabs here
                        if (string.IsNullOrWhiteSpace(currentTab) || currentTab != newTab)
                        {
                            formData.ExpandTab(formField.GetTabLabel());
                            currentTab = newTab;
                        }
                    }
                }

                if (isOnForm || (!expectedFormState.Locked.HasValue && !expectedFormState.Required.HasValue))
                {
                    // TODO: Reuse component from earlier
                    var component1 = type == "attribute"
                        ? (FormComponent)formData.Fields[row[Constants.SpecFlow.TABLE_KEY]]
                        : formData.Tabs[row[Constants.SpecFlow.TABLE_KEY]];

                    // Assert
                    AssertVisibility(component1, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Visible, errors, isOnForm);

                    var formField = component1 as FormField;
                    if (formField != null)
                    {
                        AssertReadOnly(formField, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Locked, errors);
                        AssertRequirement(formField, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Required, errors);
                    }
                }
                else
                {
                    errors.Add($"The attribute {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                }
            }
            Assert.AreEqual(0, errors.Count, string.Join(", ", errors));
        }

        private FormState GetExpectedFormState(string formStateString)
        {
            var splitted = formStateString.Split(',');

            FormState result = new FormState();
            foreach(string state in splitted)
            {
                switch(state.Trim().ToLower())
                {
                    case "required": result.Required = RequiredState.Required; break;
                    case "optional": result.Required = RequiredState.Optional; break;
                    case "recommended": result.Required = RequiredState.Recommended; break;
                    case "locked": result.Locked = true; break;
                    case "unlocked": result.Locked = false; break;
                    case "visible": result.Visible = FormVisibility.Visible; break;
                    case "invisible": result.Visible = FormVisibility.Invisible; break;
                    case "not on form": result.Visible = FormVisibility.NotOnForm; break;
                    default: throw new TestExecutionException(Constants.ErrorCodes.INVALID_FORM_STATE, state);
                }
            }
            return result;
        }

        private void AssertVisibility(FormComponent formComponent, string componentName, FormVisibility? expected, List<string> errors, bool isOnForm)
        {
            if (!expected.HasValue)
                return;

            if(isOnForm)
            {
                var isVisible = formComponent.IsVisible();
                if (expected == FormVisibility.Visible && !isVisible)
                {
                    errors.Add($"{componentName} was expected to be visible but it is invisible");
                }
                else if(expected == FormVisibility.Invisible && isVisible)
                {
                    errors.Add($"{componentName} was expected to be invisible but it is visible");
                }
                else if(expected == FormVisibility.NotOnForm)
                {
                    errors.Add($"{componentName} was shouldn't be on the form");
                }
            }
            else if(expected != FormVisibility.NotOnForm)
            {
                errors.Add($"Field {componentName} isn't on the form");
            }
        }

        private void AssertReadOnly(FormField formField, string fieldName, bool? locked, List<string> errors)
        {
            if (!locked.HasValue)
                return;

            if (formField.IsLocked() != locked.Value)
            {
                errors.Add(string.Format("{0} was expected to be {1}locked but it is {2}locked",
                   fieldName, locked.Value ? "" : "un", locked.Value ? "un" : ""));
            }
        }

        private void AssertRequirement(FormField formField, string fieldName, RequiredState? expectedRequiredState, List<string> errors)
        {
            if (!expectedRequiredState.HasValue)
                return;

            var actualRequiredState = formField.GetRequiredState();
            if (actualRequiredState != expectedRequiredState)
            {
                errors.Add($"{fieldName} was expected to be {expectedRequiredState} but it is {actualRequiredState}");
            }
        }
    }
}
