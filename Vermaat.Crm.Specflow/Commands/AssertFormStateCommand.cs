﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;
using Vermaat.Crm.Specflow.EasyRepro.Fields;

namespace Vermaat.Crm.Specflow.Commands
{
    public class AssertFormStateCommand : BrowserOnlyCommand
    {
        private readonly EntityReference _crmRecord;
        private readonly Table _visibilityCriteria;

        private class ExpectedFormState
        {
            public FormVisibility? Visible { get; set; }
            public bool? Locked { get; set; }
            public RequiredState? Required { get; set; }
        }

        public AssertFormStateCommand(CrmTestingContext crmContext, SeleniumTestingContext seleniumContext, EntityReference crmRecord, Table visibilityCriteria) :
            base(crmContext, seleniumContext)
        {
            _crmRecord = crmRecord;
            _visibilityCriteria = visibilityCriteria;
        }

        public override void Execute()
        {
            var formData = _seleniumContext.GetBrowser().OpenRecord(new OpenFormOptions(_crmRecord));
            var formState = new FormState(_seleniumContext.GetBrowser().App);
            List<string> errors = new List<string>();

            foreach (TableRow row in _visibilityCriteria.Rows)
            {
                row.TryGetValue(Constants.SpecFlow.TABLE_FORMSTATE, out var formStateTableValue);
                row.TryGetValue(Constants.SpecFlow.TABLE_TAB, out var tabTableValue);
                row.TryGetValue(Constants.SpecFlow.TABLE_SECTION, out var sectionTableValue);

                var expectedFormState = GetExpectedFormState(formStateTableValue);
                var isOnForm = formData.ContainsField(row[Constants.SpecFlow.TABLE_KEY]);

                if (isOnForm || (!expectedFormState.Locked.HasValue && !expectedFormState.Required.HasValue))
                {
                    // Assert
                    var formField = AssertTabSection(formData, formState, row[Constants.SpecFlow.TABLE_KEY], tabTableValue, sectionTableValue, errors, isOnForm);
                    AssertVisibility(formField, formState, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Visible, errors, isOnForm);

                    if (formField != null)
                    {
                        AssertReadOnly(formField, formState, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Locked, errors);
                        AssertRequirement(formField, formState, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Required, errors);
                    }
                }
                else
                {
                    errors.Add($"{row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                }

            }
            Assert.AreEqual(0, errors.Count, string.Join(", ", errors));
        }

        private FormField AssertTabSection(FormData formData, FormState formState, string fieldName, string tabName, string sectionName, List<string> errors, bool isOnForm)
        {
            if (!isOnForm)
                return null;

            if (!string.IsNullOrEmpty(sectionName))
            {
                var field = formData.GetBySection(fieldName, tabName, sectionName);
                if (field == null)
                    errors.Add($"{fieldName} can't be found in section {sectionName} within tab {tabName}");
                return field;
            }
            else if (!string.IsNullOrEmpty(tabName))
            {
                var field = formData.GetByTab(fieldName, tabName);
                if (field == null)
                    errors.Add($"{fieldName} can't be found in tab {tabName}");

                return field;
            }

            return formData[fieldName];
        }

        private ExpectedFormState GetExpectedFormState(string formStateString)
        {
            var splitted = formStateString?.Split(',') ?? Array.Empty<string>();

            ExpectedFormState result = new ExpectedFormState();
            foreach (string state in splitted)
            {
                switch (state.Trim().ToLower())
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

        private void AssertVisibility(FormField formField, FormState formState, string fieldName, FormVisibility? expected, List<string> errors, bool isOnForm)
        {
            if (!expected.HasValue)
                return;

            if (isOnForm)
            {
                if (formField == null)
                    return;

                var isVisible = formField.IsVisible(formState);
                if (expected == FormVisibility.Visible && !isVisible)
                {
                    errors.Add($"{fieldName} was expected to be visible but it is invisible");
                }
                else if (expected == FormVisibility.Invisible && isVisible)
                {
                    errors.Add($"{fieldName} was expected to be invisible but it is visible");
                }
                else if (expected == FormVisibility.NotOnForm)
                {
                    errors.Add($"{fieldName} was shouldn't be on the form");
                }
            }
            else if (expected != FormVisibility.NotOnForm)
            {
                errors.Add($"Field {fieldName} isn't on the form");
            }
        }

        private void AssertReadOnly(FormField formField, FormState formState, string fieldName, bool? locked, List<string> errors)
        {
            if (!locked.HasValue)
                return;

            if (formField.IsLocked(formState) != locked.Value)
            {
                errors.Add(string.Format("{0} was expected to be {1}locked but it is {2}locked",
                   fieldName, locked.Value ? "" : "un", locked.Value ? "un" : ""));
            }
        }

        private void AssertRequirement(FormField formField, FormState formState, string fieldName, RequiredState? expectedRequiredState, List<string> errors)
        {
            if (!expectedRequiredState.HasValue)
                return;

            var actualRequiredState = formField.GetRequiredState(formState);
            if (actualRequiredState != expectedRequiredState)
            {
                errors.Add($"{fieldName} was expected to be {expectedRequiredState} but it is {actualRequiredState}");
            }
        }
    }
}
