using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Vermaat.Crm.Specflow.EasyRepro;

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
                var expectedFormState = GetExpectedFormState(row[Constants.SpecFlow.TABLE_FORMSTATE]);
                var isOnForm = formData.ContainsField(row[Constants.SpecFlow.TABLE_KEY]);

                if (isOnForm || (!expectedFormState.Locked.HasValue && !expectedFormState.Required.HasValue))
                {
                    // Assert
                    AssertVisibility(formData, formState, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Visible, errors, isOnForm);
                    AssertReadOnly(formData, formState, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Locked, errors);
                    AssertRequirement(formData, formState, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Required, errors);
                }
                else
                {
                    errors.Add($"{row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                }
               
            }
            Assert.AreEqual(0, errors.Count, string.Join(", ", errors));
        }

        private ExpectedFormState GetExpectedFormState(string formStateString)
        {
            var splitted = formStateString.Split(',');

            ExpectedFormState result = new ExpectedFormState();
            foreach(string state in splitted)
            {
                switch(state.Trim().ToLower())
                {
                    case "required": result.Required = RequiredState.Required; break;
                    case "optional": result.Required = RequiredState.Optional; break;
                    case "recommended": throw new TestExecutionException(Constants.ErrorCodes.RECOMMENDED_NOT_SUPPORTED);
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

        private void AssertVisibility(FormData formData, FormState formState, string fieldName, FormVisibility? expected, List<string> errors, bool isOnForm)
        {
            if (!expected.HasValue)
                return;

            if(isOnForm)
            {
                var isVisible = formData[fieldName].IsVisible(formState);
                if (expected == FormVisibility.Visible && !isVisible)
                {
                    errors.Add($"{fieldName} was expected to be visible but it is invisible");
                }
                else if(expected == FormVisibility.Invisible && isVisible)
                {
                    errors.Add($"{fieldName} was expected to be invisible but it is visible");
                }
                else if(expected == FormVisibility.NotOnForm)
                {
                    errors.Add($"{fieldName} was shouldn't be on the form");
                }
            }
            else if(expected != FormVisibility.NotOnForm)
            {
                errors.Add($"Field {fieldName} isn't on the form");
            }
        }

        private void AssertReadOnly(FormData formData, FormState formState, string fieldName, bool? locked, List<string> errors)
        {
            if (!locked.HasValue)
                return;

            if (formData[fieldName].IsLocked(formState) != locked.Value)
            {
                errors.Add(string.Format("{0} was expected to be {1}locked but it is {2}locked",
                   fieldName, locked.Value ? "" : "un", locked.Value ? "un" : ""));
            }
        }

        private void AssertRequirement(FormData formData, FormState formState, string fieldName, RequiredState? expectedRequiredState, List<string> errors)
        {
            if (!expectedRequiredState.HasValue)
                return;

            var actualRequiredState = formData[fieldName].GetRequiredState(formState);
            if (actualRequiredState != expectedRequiredState)
            {
                errors.Add($"{fieldName} was expected to be {expectedRequiredState} but it is {actualRequiredState}");
            }
        }
    }
}
