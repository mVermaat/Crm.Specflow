using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk.Metadata;
using OpenQA.Selenium.Support.Extensions;
using PowerPlatform.SpecflowExtensions.EasyRepro.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlatform.SpecflowExtensions.EasyRepro
{
    public class FormStructure
    {



        //private Dictionary<string, FormField> InitializeFormData()
        //{
        //    dynamic attributeCollection = _app.WebDriver.ExecuteScript(@"
        //        return Xrm.Page.data.entity.attributes.getAll()
        //            .map(function(a) { 
        //                return { 
        //                    name: a.getName(), 
        //                    controls: a.controls.getAll().map(function(c) { 
        //                        return c.getName()
        //                    }) 
        //                } 
        //            })");

        //    var controlMap = new Dictionary<string, FormField>();
        //    var formFields = new Dictionary<string, FormField>();
        //    var metadataDic = _entityMetadata.Attributes.ToDictionary(a => a.LogicalName);
        //    foreach (var attribute in attributeCollection)
        //    {
        //        var controls = new string[attribute["controls"].Count];

        //        for (int i = 0; i < attribute["controls"].Count; i++)
        //        {
        //            controls[i] = attribute["controls"][i];
        //        }

        //        FormField field = CreateFormField(metadataDic[attribute["name"]], controls);
        //        if (field != null)
        //        {
        //            formFields.Add(attribute["name"], field);
        //            controlMap.Add(field.ControlName, field);
        //        }

        //    }

        //    dynamic formStructure = _app.WebDriver.ExecuteScript(@"
        //        return Xrm.Page.ui.tabs.getAll()
        //            .map(function(t) {
        //                return {
        //                    name: t.getName(),
        //                    label: t.getLabel(),
        //                    sections: t.sections.getAll().map(function(s) {
        //                        return {
        //                            name: s.getName(),
        //                            label: s.getLabel(),
        //                            controls: s.controls.getAll().map(function(c) {
        //                                return {
        //                                    name: c.getName(),
        //                                    type: c.getControlType()
        //                                }
        //                            })
        //                        }
        //                    })
        //                }	
        //            })");

        //    foreach (var tab in formStructure)
        //    {
        //        var formTab = new FormTab(tab["name"], tab["label"]);

        //        foreach (var section in tab["sections"])
        //        {
        //            var formSection = new FormSection(section["name"], section["label"]);
        //            formTab.Sections.Add(formSection);

        //            foreach (var control in section["controls"])
        //            {
        //                switch (control["type"])
        //                {
        //                    case "subgrid":
        //                        var grid = new Subgrid(_app, control["name"]);
        //                        formSection.Subgrids.Add(grid);
        //                        break;
        //                    case "standard":
        //                    case "lookup":
        //                    case "optionset":
        //                    case "multiselectoptionset":
        //                        if (controlMap.TryGetValue(control["name"], out FormField formField))
        //                            formSection.Fields.Add(formField);
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    return formFields;
        //}

        //private FormField CreateFormField(AttributeMetadata metadata, string[] controls)
        //{
        //    if (controls.Length == 0)
        //        return null;

        //    // Take the first control that isn't in the header
        //    for (int i = 0; i < controls.Length; i++)
        //    {
        //        if (!controls[i].StartsWith("header"))
        //        {
        //            return new BodyFormField(_app, metadata, controls[i]);
        //        }
        //    }
        //    // If all are in the header, take the first header control
        //    return new HeaderFormField(_app, metadata, controls[0]);
        //}

        internal static FormStructure FromCurrentScreen(SeleniumExecutor executor, EntityMetadata metadata)
        {
            executor.Execute("Build form structure", (driver, selectors) =>
            {
                dynamic attributeCollection = driver.ExecuteScript(Constants.JavaScriptCommands.GET_FORM_ATTRIBUTES);

                var controlMap = new Dictionary<string, FormField>();
                var formFields = new Dictionary<string, FormField>();
                var metadataDic = metadata.Attributes.ToDictionary(a => a.LogicalName);
                foreach (var attribute in attributeCollection)
                {
                    var controls = new string[attribute["controls"].Count];

                    for (int i = 0; i < attribute["controls"].Count; i++)
                    {
                        controls[i] = attribute["controls"][i];
                    }

                    FormField field = CreateFormField(metadataDic[attribute["name"]], controls);
                    if (field != null)
                    {
                        formFields.Add(attribute["name"], field);
                        controlMap.Add(field.ControlName, field);
                    }

                }

                dynamic formStructure = driver.ExecuteScript(Constants.JavaScriptCommands.GET_FORM_STRUCTURE);

                foreach (var tab in formStructure)
                {
                    var formTab = new FormTab(tab["name"], tab["label"]);

                    foreach (var section in tab["sections"])
                    {
                        var formSection = new FormSection(section["name"], section["label"]);
                        formTab.Sections.Add(formSection);

                        foreach (var control in section["controls"])
                        {
                            switch (control["type"])
                            {
                                case "subgrid":
                                    var grid = new Subgrid(_app, control["name"]);
                                    formSection.Subgrids.Add(grid);
                                    break;
                                case "standard":
                                case "lookup":
                                case "optionset":
                                case "multiselectoptionset":
                                    if (controlMap.TryGetValue(control["name"], out FormField formField))
                                        formSection.Fields.Add(formField);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                return formFields;
            });
        }
    }
}

