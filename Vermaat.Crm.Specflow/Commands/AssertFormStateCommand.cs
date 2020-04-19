using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
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
                FormComponent component = GetComponent(row, formData, out var controlType);

                var isOnForm = component != null;

                if (isOnForm)
                {
                    var newTab = component.GetTabName();
                    if (string.IsNullOrWhiteSpace(currentTab) || currentTab != newTab)
                    {
                        formData.ExpandTab(component.GetTabLabel());
                        currentTab = newTab;
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
                    errors.Add($"The {controlType} {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                }
            }
            Assert.AreEqual(0, errors.Count, string.Join(", ", errors));
        }

        private static FormComponent GetComponent(TableRow row, FormData formData, out ControlType type)
        {
            if (!row.ContainsKey(Constants.SpecFlow.TABLE_TYPE))
                type = ControlType.attribute;
            else if (!Enum.TryParse(row[Constants.SpecFlow.TABLE_TYPE], out type))
                throw new TestExecutionException(Constants.ErrorCodes.COMPONENT_TYPE_NOT_RECOGNIZED, row[Constants.SpecFlow.TABLE_TYPE]);

            var key = row[Constants.SpecFlow.TABLE_KEY];

            switch (type)
            {
                case ControlType.attribute:
                    return formData.Fields.FindByName(key);
                case ControlType.tab:
                    if (!row.ContainsKey(Constants.SpecFlow.TABLE_TAB))
                        throw new TestExecutionException(Constants.ErrorCodes.TAB_NOT_SPECIFIED, type.ToString());
                    var tabName = row[Constants.SpecFlow.TABLE_TAB];

                    return formData.Tabs.Find(tabName).FirstOrDefault();
                case ControlType.section:
                    if (!row.ContainsKey(Constants.SpecFlow.TABLE_TAB))
                        throw new TestExecutionException(Constants.ErrorCodes.TAB_NOT_SPECIFIED, type.ToString());
                    if (!row.ContainsKey(Constants.SpecFlow.TABLE_SECTION))
                        throw new TestExecutionException(Constants.ErrorCodes.SECTION_NOT_SPECIFIED, type.ToString());

                    var tabNameForSection = row[Constants.SpecFlow.TABLE_TAB];
                    var sectionName = row[Constants.SpecFlow.TABLE_SECTION];

                    var tab = formData.Tabs.Find(tabNameForSection).FirstOrDefault();
                    if (tab == null)
                        throw new TestExecutionException(Constants.ErrorCodes.TAB_NOT_FOUND, tabNameForSection);

                    return tab.Sections.Find(sectionName).FirstOrDefault();
                case ControlType.subgrid:
                    return formData.Subgrids.Find(key).FirstOrDefault();
                default:
                    throw new TestExecutionException(Constants.ErrorCodes.UNSUPPORTED_COMPONENT_TYPE, type.ToString());
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
