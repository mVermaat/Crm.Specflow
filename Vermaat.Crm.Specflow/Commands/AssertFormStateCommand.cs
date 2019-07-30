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
                // Check if field is on form & select correct tab
                Assert.IsTrue(formData.ContainsField(row[Constants.SpecFlow.TABLE_KEY]), $"Field {row[Constants.SpecFlow.TABLE_KEY]} isn't on the form");
                var field = formData[row[Constants.SpecFlow.TABLE_KEY]];
                var newTab = field.GetTabName();
                if (string.IsNullOrWhiteSpace(currentTab) || currentTab != newTab)
                {
                    formData.ExpandTab(field.GetTabLabel());
                    currentTab = newTab;
                }

                // Assert
                var expectedFormState = GetExpectedFormState(row[Constants.SpecFlow.TABLE_FORMSTATE]);
                AssertVisibility(formData, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Visible, errors);
                AssertReadOnly(formData, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Locked, errors);
                AssertRequirement(formData, row[Constants.SpecFlow.TABLE_KEY], expectedFormState.Required, errors);
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
                    case "visible": result.Visible = true; break;
                    case "invisible": result.Visible = false; break;
                    default: throw new TestExecutionException(Constants.ErrorCodes.INVALID_FORM_STATE, state);
                }
            }
            return result;
        }

        private void AssertVisibility(FormData formData, string fieldName, bool? expected, List<string> errors)
        {
            if (!expected.HasValue)
                return;

            if (formData[fieldName].IsVisible() != expected.Value)
            {
                errors.Add(string.Format("{0} was expected to be {1}visible but it is {2}visible",
                   fieldName, expected.Value ? "" : "in", expected.Value ? "in" : ""));
            }
        }

        private void AssertReadOnly(FormData formData, string fieldName, bool? locked, List<string> errors)
        {
            if (!locked.HasValue)
                return;

            if (formData[fieldName].IsLocked() != locked.Value)
            {
                errors.Add(string.Format("{0} was expected to be {1}locked but it is {2}locked",
                   fieldName, locked.Value ? "" : "un", locked.Value ? "un" : ""));
            }
        }

        private void AssertRequirement(FormData formData, string fieldName, RequiredState? expectedRequiredState, List<string> errors)
        {
            if (!expectedRequiredState.HasValue)
                return;

            var actualRequiredState = formData[fieldName].GetRequiredState();
            if (actualRequiredState != expectedRequiredState)
            {
                errors.Add($"{fieldName} was expected to be {expectedRequiredState} but it is {actualRequiredState}");
            }
        }
    }
}
