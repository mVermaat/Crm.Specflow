using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace Vermaat.Crm.Specflow.Entities
{
    public static class FormXmlDefinitionExtensions
    {
        public static string GetLabelInLanguage(this FormLabel[] labels, int languageCode, int fallbackLanguage)
            => labels.FirstOrDefault(l => l.LanguageCode == languageCode     && !string.IsNullOrEmpty(l.Label))?.Label 
            ?? labels.FirstOrDefault(l => l.LanguageCode == fallbackLanguage && !string.IsNullOrEmpty(l.Label))?.Label;
    }


    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot("form", Namespace = "", IsNullable = false)]
    public class FormXmlDefinition
    {

        [XmlArrayItem("Role", IsNullable = false)]
        public FormRole[] DisplayConditions { get; set; }

        [XmlArray("tabs")]
        [XmlArrayItem("tab", IsNullable = false)]
        public FormTab[] Tabs { get; set; }

        [XmlElement("header")]
        public FormHeader Header { get; set; }

}

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class FormHeader
    {

        [XmlArray("rows")]
        [XmlArrayItem("row", IsNullable = false)]
        public FormRow[] Rows { get; set; }
    }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class FormRole
    {
        [XmlAttribute()]
        public string Id { get; set; }
    }


    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class FormTab
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("labels")]
        [XmlArrayItem("label", IsNullable = false)]
        public FormLabel[] Labels { get; set; }

        [XmlArray("columns")]
        [XmlArrayItem("column", IsNullable = false)]
        public FormColumn[] Columns { get; set; }
    }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class FormColumn
    {
       
        [XmlArray("sections")]
        [XmlArrayItem("section", IsNullable = false)]
        public FormSection[] Sections { get; set; }
    }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class FormSection
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("rows")]
        [XmlArrayItem("row", IsNullable = false)]
        public FormRow[] Rows { get; set; }

        [XmlArray("labels")]
        [XmlArrayItem("label", IsNullable = false)]
        public FormLabel[] Labels { get; set; }
    }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class FormRow
    {
        [XmlElement("cell", IsNullable = false)]
        public FormCell[] Cells { get; set; }
    }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class FormCell
    {

        [XmlArray("labels")]
        [XmlArrayItem("label", IsNullable = false)]
        public FormLabel[] Labels { get; set; }

        [XmlElement("control")]
        public FormControl Control { get; set; }

        [XmlAttribute("userspacer")]
        public bool IsSpacer { get; set; }
    }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class FormControl
    {
        [XmlAttribute("id")]
        public string ControlName { get; set; }

        [XmlAttribute("datafieldname")]
        public string AttributeName { get; set; }

    }

    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public class FormLabel
    {
        [XmlAttribute("description")]
        public string Label { get; set; }

        [XmlAttribute("languagecode")]
        public int LanguageCode { get; set; }

    }



}
