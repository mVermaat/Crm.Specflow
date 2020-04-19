using Microsoft.Crm.Sdk.Messages;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.UI;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Tracing;
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
                FormComponent component = GetComponent(row, formData);

                // TODO: Move this to a separate function and throw exception if the type is not recognized
                var isOnForm = component != null;

                if (isOnForm)
                {
                    var formField = component as FormField;
                    if (formField != null)
                    {
                        var newTab = formField.GetTabName();
                        if (string.IsNullOrWhiteSpace(currentTab) || currentTab != newTab)
                        {
                            formData.ExpandTab(formField.GetTabLabel());
                            currentTab = newTab;
                        }
                    }
                }

                if (isOnForm || (!expectedFormState.Locked.HasValue && !expectedFormState.Required.HasValue))
                {
                    // Assert
                    AssertVisibility(component, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Visible, errors, isOnForm);

                    var formField = component as FormField;
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

        private static FormComponent GetComponent(TableRow row, FormData formData)
        {
            var type = row.ContainsKey("Type") ? row["Type"] : "attribute";
            var key = row[Constants.SpecFlow.TABLE_KEY];

            switch (type)
            {
                case "attribute":
                    return formData.ContainsField(key) ? formData.Fields[key] : null;
                case "tab":
                    if (!row.ContainsKey("Tab")) // TODO: Use constants for all header values
                        throw new Exception("The tab must be specified when checking tab visibility.");
                    var tabName = row["Tab"];

                    return formData.ContainsTab(tabName) ? formData.Tabs[tabName] : null;
                case "section":
                    // TODO: Throw proper exceptions here
                    if (!row.ContainsKey("Tab")) // TODO: Use constants for all header values
                        throw new Exception("The tab must be specified when checking section visibility."); 
                    if (!row.ContainsKey("Section"))
                        throw new Exception("The section must be specified when checking section visibility.");

                    var tabName1 = row["Tab"];
                    var sectionName = row["Section"];

                    if (!formData.Tabs.ContainsKey(tabName1))
                        throw new Exception("The tab was not found.");
                    var tab = formData.Tabs[tabName1];

                    return tab.Sections.ContainsKey(sectionName) ? tab.Sections[sectionName] : null;
                case "subgrid":
                    return formData.Subgrids.ContainsKey(key.ToLower()) ? formData.Subgrids[key.ToLower()] : null;
                case null:
                    return null;
                default:
                    // TODO: Change this to correct exception type
                    throw new Exception("Unrecognized type");
            }
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
