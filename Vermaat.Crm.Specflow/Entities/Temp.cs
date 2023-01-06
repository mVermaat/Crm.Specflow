using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.Entities
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class form
    {

        private formAncestor ancestorField;

        private formData[] hiddencontrolsField;

        private formTab[] tabsField;

        private formEvents eventsField;

        private formHeader headerField;

        private formClientresources clientresourcesField;

        private formNavigation navigationField;

        private formControlDescriptions controlDescriptionsField;

        private formDisplayConditions displayConditionsField;

        private formLibrary[] formLibrariesField;

        private bool showImageField;

        private bool shownavigationbarField;

        private ushort maxWidthField;

        /// <remarks/>
        public formAncestor ancestor
        {
            get
            {
                return this.ancestorField;
            }
            set
            {
                this.ancestorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("data", IsNullable = false)]
        public formData[] hiddencontrols
        {
            get
            {
                return this.hiddencontrolsField;
            }
            set
            {
                this.hiddencontrolsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("tab", IsNullable = false)]
        public formTab[] tabs
        {
            get
            {
                return this.tabsField;
            }
            set
            {
                this.tabsField = value;
            }
        }

        /// <remarks/>
        public formEvents events
        {
            get
            {
                return this.eventsField;
            }
            set
            {
                this.eventsField = value;
            }
        }

        /// <remarks/>
        public formHeader header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public formClientresources clientresources
        {
            get
            {
                return this.clientresourcesField;
            }
            set
            {
                this.clientresourcesField = value;
            }
        }

        /// <remarks/>
        public formNavigation Navigation
        {
            get
            {
                return this.navigationField;
            }
            set
            {
                this.navigationField = value;
            }
        }

        /// <remarks/>
        public formControlDescriptions controlDescriptions
        {
            get
            {
                return this.controlDescriptionsField;
            }
            set
            {
                this.controlDescriptionsField = value;
            }
        }

        /// <remarks/>
        public formDisplayConditions DisplayConditions
        {
            get
            {
                return this.displayConditionsField;
            }
            set
            {
                this.displayConditionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Library", IsNullable = false)]
        public formLibrary[] formLibraries
        {
            get
            {
                return this.formLibrariesField;
            }
            set
            {
                this.formLibrariesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool showImage
        {
            get
            {
                return this.showImageField;
            }
            set
            {
                this.showImageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool shownavigationbar
        {
            get
            {
                return this.shownavigationbarField;
            }
            set
            {
                this.shownavigationbarField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort maxWidth
        {
            get
            {
                return this.maxWidthField;
            }
            set
            {
                this.maxWidthField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formAncestor
    {

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formData
    {

        private string idField;

        private string datafieldnameField;

        private string classidField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string datafieldname
        {
            get
            {
                return this.datafieldnameField;
            }
            set
            {
                this.datafieldnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classid
        {
            get
            {
                return this.classidField;
            }
            set
            {
                this.classidField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTab
    {

        private formTabLabels labelsField;

        private formTabColumn[] columnsField;

        private string nameField;

        private string idField;

        private byte isUserDefinedField;

        private bool showlabelField;

        private bool expandedField;

        private byte locklevelField;

        private bool locklevelFieldSpecified;

        /// <remarks/>
        public formTabLabels labels
        {
            get
            {
                return this.labelsField;
            }
            set
            {
                this.labelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("column", IsNullable = false)]
        public formTabColumn[] columns
        {
            get
            {
                return this.columnsField;
            }
            set
            {
                this.columnsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte IsUserDefined
        {
            get
            {
                return this.isUserDefinedField;
            }
            set
            {
                this.isUserDefinedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool showlabel
        {
            get
            {
                return this.showlabelField;
            }
            set
            {
                this.showlabelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool expanded
        {
            get
            {
                return this.expandedField;
            }
            set
            {
                this.expandedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte locklevel
        {
            get
            {
                return this.locklevelField;
            }
            set
            {
                this.locklevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool locklevelSpecified
        {
            get
            {
                return this.locklevelFieldSpecified;
            }
            set
            {
                this.locklevelFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabLabels
    {

        private formTabLabelsLabel labelField;

        /// <remarks/>
        public formTabLabelsLabel label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabLabelsLabel
    {

        private string descriptionField;

        private ushort languagecodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort languagecode
        {
            get
            {
                return this.languagecodeField;
            }
            set
            {
                this.languagecodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumn
    {

        private formTabColumnSection[] sectionsField;

        private string widthField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("section", IsNullable = false)]
        public formTabColumnSection[] sections
        {
            get
            {
                return this.sectionsField;
            }
            set
            {
                this.sectionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSection
    {

        private formTabColumnSectionLabels labelsField;

        private formTabColumnSectionRow[] rowsField;

        private string nameField;

        private bool showlabelField;

        private bool showbarField;

        private string idField;

        private byte isUserDefinedField;

        private string layoutField;

        private byte columnsField;

        private byte labelwidthField;

        private bool labelwidthFieldSpecified;

        private string celllabelpositionField;

        private byte locklevelField;

        private bool locklevelFieldSpecified;

        private string celllabelalignmentField;

        private string heightField;

        /// <remarks/>
        public formTabColumnSectionLabels labels
        {
            get
            {
                return this.labelsField;
            }
            set
            {
                this.labelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("row", IsNullable = false)]
        public formTabColumnSectionRow[] rows
        {
            get
            {
                return this.rowsField;
            }
            set
            {
                this.rowsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool showlabel
        {
            get
            {
                return this.showlabelField;
            }
            set
            {
                this.showlabelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool showbar
        {
            get
            {
                return this.showbarField;
            }
            set
            {
                this.showbarField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte IsUserDefined
        {
            get
            {
                return this.isUserDefinedField;
            }
            set
            {
                this.isUserDefinedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string layout
        {
            get
            {
                return this.layoutField;
            }
            set
            {
                this.layoutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte columns
        {
            get
            {
                return this.columnsField;
            }
            set
            {
                this.columnsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte labelwidth
        {
            get
            {
                return this.labelwidthField;
            }
            set
            {
                this.labelwidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool labelwidthSpecified
        {
            get
            {
                return this.labelwidthFieldSpecified;
            }
            set
            {
                this.labelwidthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string celllabelposition
        {
            get
            {
                return this.celllabelpositionField;
            }
            set
            {
                this.celllabelpositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte locklevel
        {
            get
            {
                return this.locklevelField;
            }
            set
            {
                this.locklevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool locklevelSpecified
        {
            get
            {
                return this.locklevelFieldSpecified;
            }
            set
            {
                this.locklevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string celllabelalignment
        {
            get
            {
                return this.celllabelalignmentField;
            }
            set
            {
                this.celllabelalignmentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionLabels
    {

        private formTabColumnSectionLabelsLabel labelField;

        /// <remarks/>
        public formTabColumnSectionLabelsLabel label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionLabelsLabel
    {

        private string descriptionField;

        private ushort languagecodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort languagecode
        {
            get
            {
                return this.languagecodeField;
            }
            set
            {
                this.languagecodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRow
    {

        private formTabColumnSectionRowCell cellField;

        private string heightField;

        /// <remarks/>
        public formTabColumnSectionRowCell cell
        {
            get
            {
                return this.cellField;
            }
            set
            {
                this.cellField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRowCell
    {

        private formTabColumnSectionRowCellLabels labelsField;

        private formTabColumnSectionRowCellEvents eventsField;

        private formTabColumnSectionRowCellControl controlField;

        private string idField;

        private bool showlabelField;

        private bool showlabelFieldSpecified;

        private byte locklevelField;

        private bool locklevelFieldSpecified;

        private byte rowspanField;

        private bool rowspanFieldSpecified;

        private byte colspanField;

        private bool colspanFieldSpecified;

        private bool autoField;

        private bool autoFieldSpecified;

        private bool visibleField;

        private bool visibleFieldSpecified;

        private bool userspacerField;

        private bool userspacerFieldSpecified;

        /// <remarks/>
        public formTabColumnSectionRowCellLabels labels
        {
            get
            {
                return this.labelsField;
            }
            set
            {
                this.labelsField = value;
            }
        }

        /// <remarks/>
        public formTabColumnSectionRowCellEvents events
        {
            get
            {
                return this.eventsField;
            }
            set
            {
                this.eventsField = value;
            }
        }

        /// <remarks/>
        public formTabColumnSectionRowCellControl control
        {
            get
            {
                return this.controlField;
            }
            set
            {
                this.controlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool showlabel
        {
            get
            {
                return this.showlabelField;
            }
            set
            {
                this.showlabelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool showlabelSpecified
        {
            get
            {
                return this.showlabelFieldSpecified;
            }
            set
            {
                this.showlabelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte locklevel
        {
            get
            {
                return this.locklevelField;
            }
            set
            {
                this.locklevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool locklevelSpecified
        {
            get
            {
                return this.locklevelFieldSpecified;
            }
            set
            {
                this.locklevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte rowspan
        {
            get
            {
                return this.rowspanField;
            }
            set
            {
                this.rowspanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool rowspanSpecified
        {
            get
            {
                return this.rowspanFieldSpecified;
            }
            set
            {
                this.rowspanFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte colspan
        {
            get
            {
                return this.colspanField;
            }
            set
            {
                this.colspanField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool colspanSpecified
        {
            get
            {
                return this.colspanFieldSpecified;
            }
            set
            {
                this.colspanFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool auto
        {
            get
            {
                return this.autoField;
            }
            set
            {
                this.autoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool autoSpecified
        {
            get
            {
                return this.autoFieldSpecified;
            }
            set
            {
                this.autoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool visible
        {
            get
            {
                return this.visibleField;
            }
            set
            {
                this.visibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool visibleSpecified
        {
            get
            {
                return this.visibleFieldSpecified;
            }
            set
            {
                this.visibleFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool userspacer
        {
            get
            {
                return this.userspacerField;
            }
            set
            {
                this.userspacerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool userspacerSpecified
        {
            get
            {
                return this.userspacerFieldSpecified;
            }
            set
            {
                this.userspacerFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRowCellLabels
    {

        private formTabColumnSectionRowCellLabelsLabel labelField;

        /// <remarks/>
        public formTabColumnSectionRowCellLabelsLabel label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRowCellLabelsLabel
    {

        private string descriptionField;

        private ushort languagecodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort languagecode
        {
            get
            {
                return this.languagecodeField;
            }
            set
            {
                this.languagecodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRowCellEvents
    {

        private formTabColumnSectionRowCellEventsEvent eventField;

        /// <remarks/>
        public formTabColumnSectionRowCellEventsEvent @event
        {
            get
            {
                return this.eventField;
            }
            set
            {
                this.eventField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRowCellEventsEvent
    {

        private formTabColumnSectionRowCellEventsEventInternalHandlers internalHandlersField;

        private string nameField;

        private bool applicationField;

        private bool activeField;

        /// <remarks/>
        public formTabColumnSectionRowCellEventsEventInternalHandlers InternalHandlers
        {
            get
            {
                return this.internalHandlersField;
            }
            set
            {
                this.internalHandlersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool application
        {
            get
            {
                return this.applicationField;
            }
            set
            {
                this.applicationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool active
        {
            get
            {
                return this.activeField;
            }
            set
            {
                this.activeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRowCellEventsEventInternalHandlers
    {

        private formTabColumnSectionRowCellEventsEventInternalHandlersHandler handlerField;

        /// <remarks/>
        public formTabColumnSectionRowCellEventsEventInternalHandlersHandler Handler
        {
            get
            {
                return this.handlerField;
            }
            set
            {
                this.handlerField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRowCellEventsEventInternalHandlersHandler
    {

        private string functionNameField;

        private string libraryNameField;

        private string handlerUniqueIdField;

        private bool enabledField;

        private bool passExecutionContextField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string functionName
        {
            get
            {
                return this.functionNameField;
            }
            set
            {
                this.functionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string libraryName
        {
            get
            {
                return this.libraryNameField;
            }
            set
            {
                this.libraryNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string handlerUniqueId
        {
            get
            {
                return this.handlerUniqueIdField;
            }
            set
            {
                this.handlerUniqueIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool enabled
        {
            get
            {
                return this.enabledField;
            }
            set
            {
                this.enabledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool passExecutionContext
        {
            get
            {
                return this.passExecutionContextField;
            }
            set
            {
                this.passExecutionContextField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRowCellControl
    {

        private formTabColumnSectionRowCellControlParameters parametersField;

        private string idField;

        private string classidField;

        private string datafieldnameField;

        private bool disabledField;

        private bool disabledFieldSpecified;

        private string uniqueidField;

        private bool indicationOfSubgridField;

        private bool indicationOfSubgridFieldSpecified;

        /// <remarks/>
        public formTabColumnSectionRowCellControlParameters parameters
        {
            get
            {
                return this.parametersField;
            }
            set
            {
                this.parametersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classid
        {
            get
            {
                return this.classidField;
            }
            set
            {
                this.classidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string datafieldname
        {
            get
            {
                return this.datafieldnameField;
            }
            set
            {
                this.datafieldnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool disabled
        {
            get
            {
                return this.disabledField;
            }
            set
            {
                this.disabledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool disabledSpecified
        {
            get
            {
                return this.disabledFieldSpecified;
            }
            set
            {
                this.disabledFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string uniqueid
        {
            get
            {
                return this.uniqueidField;
            }
            set
            {
                this.uniqueidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool indicationOfSubgrid
        {
            get
            {
                return this.indicationOfSubgridField;
            }
            set
            {
                this.indicationOfSubgridField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool indicationOfSubgridSpecified
        {
            get
            {
                return this.indicationOfSubgridFieldSpecified;
            }
            set
            {
                this.indicationOfSubgridFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formTabColumnSectionRowCellControlParameters
    {

        private object[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AddressField", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("AllowFilterOff", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("AutoExpand", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("AutoResolve", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("ChartGridMode", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("ControlMode", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("DefaultViewId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("DependentAttributeName", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("DependentAttributeType", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("DisableMru", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("DisableQuickFind", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("DisableViewPicker", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("EnableChartPicker", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("EnableContextualActions", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("EnableJumpBar", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("EnableQuickFind", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("EnableViewPicker", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("FilterRelationshipName", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("IsUserChart", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("IsUserView", typeof(bool))]
        [System.Xml.Serialization.XmlElementAttribute("QuickForms", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("RecordsPerPage", typeof(byte))]
        [System.Xml.Serialization.XmlElementAttribute("RelationshipName", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("TargetEntityType", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("ViewId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("ViewIds", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("VisualizationId", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        AddressField,

        /// <remarks/>
        AllowFilterOff,

        /// <remarks/>
        AutoExpand,

        /// <remarks/>
        AutoResolve,

        /// <remarks/>
        ChartGridMode,

        /// <remarks/>
        ControlMode,

        /// <remarks/>
        DefaultViewId,

        /// <remarks/>
        DependentAttributeName,

        /// <remarks/>
        DependentAttributeType,

        /// <remarks/>
        DisableMru,

        /// <remarks/>
        DisableQuickFind,

        /// <remarks/>
        DisableViewPicker,

        /// <remarks/>
        EnableChartPicker,

        /// <remarks/>
        EnableContextualActions,

        /// <remarks/>
        EnableJumpBar,

        /// <remarks/>
        EnableQuickFind,

        /// <remarks/>
        EnableViewPicker,

        /// <remarks/>
        FilterRelationshipName,

        /// <remarks/>
        IsUserChart,

        /// <remarks/>
        IsUserView,

        /// <remarks/>
        QuickForms,

        /// <remarks/>
        RecordsPerPage,

        /// <remarks/>
        RelationshipName,

        /// <remarks/>
        TargetEntityType,

        /// <remarks/>
        ViewId,

        /// <remarks/>
        ViewIds,

        /// <remarks/>
        VisualizationId,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formEvents
    {

        private formEventsEvent eventField;

        /// <remarks/>
        public formEventsEvent @event
        {
            get
            {
                return this.eventField;
            }
            set
            {
                this.eventField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formEventsEvent
    {

        private formEventsEventInternalHandlers internalHandlersField;

        private formEventsEventHandler[] handlersField;

        private string nameField;

        private bool applicationField;

        private bool activeField;

        /// <remarks/>
        public formEventsEventInternalHandlers InternalHandlers
        {
            get
            {
                return this.internalHandlersField;
            }
            set
            {
                this.internalHandlersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Handler", IsNullable = false)]
        public formEventsEventHandler[] Handlers
        {
            get
            {
                return this.handlersField;
            }
            set
            {
                this.handlersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool application
        {
            get
            {
                return this.applicationField;
            }
            set
            {
                this.applicationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool active
        {
            get
            {
                return this.activeField;
            }
            set
            {
                this.activeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formEventsEventInternalHandlers
    {

        private formEventsEventInternalHandlersHandler handlerField;

        /// <remarks/>
        public formEventsEventInternalHandlersHandler Handler
        {
            get
            {
                return this.handlerField;
            }
            set
            {
                this.handlerField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formEventsEventInternalHandlersHandler
    {

        private string functionNameField;

        private string libraryNameField;

        private string handlerUniqueIdField;

        private bool enabledField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string functionName
        {
            get
            {
                return this.functionNameField;
            }
            set
            {
                this.functionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string libraryName
        {
            get
            {
                return this.libraryNameField;
            }
            set
            {
                this.libraryNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string handlerUniqueId
        {
            get
            {
                return this.handlerUniqueIdField;
            }
            set
            {
                this.handlerUniqueIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool enabled
        {
            get
            {
                return this.enabledField;
            }
            set
            {
                this.enabledField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formEventsEventHandler
    {

        private string functionNameField;

        private string libraryNameField;

        private string handlerUniqueIdField;

        private bool enabledField;

        private string parametersField;

        private bool passExecutionContextField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string functionName
        {
            get
            {
                return this.functionNameField;
            }
            set
            {
                this.functionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string libraryName
        {
            get
            {
                return this.libraryNameField;
            }
            set
            {
                this.libraryNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string handlerUniqueId
        {
            get
            {
                return this.handlerUniqueIdField;
            }
            set
            {
                this.handlerUniqueIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool enabled
        {
            get
            {
                return this.enabledField;
            }
            set
            {
                this.enabledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parameters
        {
            get
            {
                return this.parametersField;
            }
            set
            {
                this.parametersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool passExecutionContext
        {
            get
            {
                return this.passExecutionContextField;
            }
            set
            {
                this.passExecutionContextField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formHeader
    {

        private formHeaderRows rowsField;

        private string idField;

        private byte columnsField;

        private string celllabelpositionField;

        private byte labelwidthField;

        /// <remarks/>
        public formHeaderRows rows
        {
            get
            {
                return this.rowsField;
            }
            set
            {
                this.rowsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte columns
        {
            get
            {
                return this.columnsField;
            }
            set
            {
                this.columnsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string celllabelposition
        {
            get
            {
                return this.celllabelpositionField;
            }
            set
            {
                this.celllabelpositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte labelwidth
        {
            get
            {
                return this.labelwidthField;
            }
            set
            {
                this.labelwidthField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formHeaderRows
    {

        private formHeaderRowsCell[] rowField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("cell", IsNullable = false)]
        public formHeaderRowsCell[] row
        {
            get
            {
                return this.rowField;
            }
            set
            {
                this.rowField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formHeaderRowsCell
    {

        private formHeaderRowsCellLabels labelsField;

        private formHeaderRowsCellControl controlField;

        private string idField;

        private bool showlabelField;

        private bool showlabelFieldSpecified;

        private byte locklevelField;

        private bool locklevelFieldSpecified;

        /// <remarks/>
        public formHeaderRowsCellLabels labels
        {
            get
            {
                return this.labelsField;
            }
            set
            {
                this.labelsField = value;
            }
        }

        /// <remarks/>
        public formHeaderRowsCellControl control
        {
            get
            {
                return this.controlField;
            }
            set
            {
                this.controlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool showlabel
        {
            get
            {
                return this.showlabelField;
            }
            set
            {
                this.showlabelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool showlabelSpecified
        {
            get
            {
                return this.showlabelFieldSpecified;
            }
            set
            {
                this.showlabelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte locklevel
        {
            get
            {
                return this.locklevelField;
            }
            set
            {
                this.locklevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool locklevelSpecified
        {
            get
            {
                return this.locklevelFieldSpecified;
            }
            set
            {
                this.locklevelFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formHeaderRowsCellLabels
    {

        private formHeaderRowsCellLabelsLabel labelField;

        /// <remarks/>
        public formHeaderRowsCellLabelsLabel label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formHeaderRowsCellLabelsLabel
    {

        private string descriptionField;

        private ushort languagecodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort languagecode
        {
            get
            {
                return this.languagecodeField;
            }
            set
            {
                this.languagecodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formHeaderRowsCellControl
    {

        private string idField;

        private string classidField;

        private string datafieldnameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string classid
        {
            get
            {
                return this.classidField;
            }
            set
            {
                this.classidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string datafieldname
        {
            get
            {
                return this.datafieldnameField;
            }
            set
            {
                this.datafieldnameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formClientresources
    {

        private formClientresourcesInternalresources internalresourcesField;

        /// <remarks/>
        public formClientresourcesInternalresources internalresources
        {
            get
            {
                return this.internalresourcesField;
            }
            set
            {
                this.internalresourcesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formClientresourcesInternalresources
    {

        private formClientresourcesInternalresourcesInternaljscriptfile[] clientincludesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("internaljscriptfile", IsNullable = false)]
        public formClientresourcesInternalresourcesInternaljscriptfile[] clientincludes
        {
            get
            {
                return this.clientincludesField;
            }
            set
            {
                this.clientincludesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formClientresourcesInternalresourcesInternaljscriptfile
    {

        private string srcField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string src
        {
            get
            {
                return this.srcField;
            }
            set
            {
                this.srcField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formNavigation
    {

        private formNavigationNavBarByRelationshipItem[] navBarField;

        private formNavigationNavBarArea[] navBarAreasField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NavBarByRelationshipItem", IsNullable = false)]
        public formNavigationNavBarByRelationshipItem[] NavBar
        {
            get
            {
                return this.navBarField;
            }
            set
            {
                this.navBarField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NavBarArea", IsNullable = false)]
        public formNavigationNavBarArea[] NavBarAreas
        {
            get
            {
                return this.navBarAreasField;
            }
            set
            {
                this.navBarAreasField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formNavigationNavBarByRelationshipItem
    {

        private formNavigationNavBarByRelationshipItemPrivileges privilegesField;

        private formNavigationNavBarByRelationshipItemTitle[] titlesField;

        private string relationshipNameField;

        private string idField;

        private ushort sequenceField;

        private string areaField;

        private bool showField;

        private string titleResourceIdField;

        /// <remarks/>
        public formNavigationNavBarByRelationshipItemPrivileges Privileges
        {
            get
            {
                return this.privilegesField;
            }
            set
            {
                this.privilegesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Title", IsNullable = false)]
        public formNavigationNavBarByRelationshipItemTitle[] Titles
        {
            get
            {
                return this.titlesField;
            }
            set
            {
                this.titlesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RelationshipName
        {
            get
            {
                return this.relationshipNameField;
            }
            set
            {
                this.relationshipNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Sequence
        {
            get
            {
                return this.sequenceField;
            }
            set
            {
                this.sequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Area
        {
            get
            {
                return this.areaField;
            }
            set
            {
                this.areaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Show
        {
            get
            {
                return this.showField;
            }
            set
            {
                this.showField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TitleResourceId
        {
            get
            {
                return this.titleResourceIdField;
            }
            set
            {
                this.titleResourceIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formNavigationNavBarByRelationshipItemPrivileges
    {

        private formNavigationNavBarByRelationshipItemPrivilegesPrivilege privilegeField;

        /// <remarks/>
        public formNavigationNavBarByRelationshipItemPrivilegesPrivilege Privilege
        {
            get
            {
                return this.privilegeField;
            }
            set
            {
                this.privilegeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formNavigationNavBarByRelationshipItemPrivilegesPrivilege
    {

        private string entityField;

        private string privilegeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Entity
        {
            get
            {
                return this.entityField;
            }
            set
            {
                this.entityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Privilege
        {
            get
            {
                return this.privilegeField;
            }
            set
            {
                this.privilegeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formNavigationNavBarByRelationshipItemTitle
    {

        private ushort lCIDField;

        private string textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort LCID
        {
            get
            {
                return this.lCIDField;
            }
            set
            {
                this.lCIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formNavigationNavBarArea
    {

        private formNavigationNavBarAreaTitles titlesField;

        private string idField;

        /// <remarks/>
        public formNavigationNavBarAreaTitles Titles
        {
            get
            {
                return this.titlesField;
            }
            set
            {
                this.titlesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formNavigationNavBarAreaTitles
    {

        private formNavigationNavBarAreaTitlesTitle titleField;

        /// <remarks/>
        public formNavigationNavBarAreaTitlesTitle Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formNavigationNavBarAreaTitlesTitle
    {

        private string textField;

        private ushort lCIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort LCID
        {
            get
            {
                return this.lCIDField;
            }
            set
            {
                this.lCIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formControlDescriptions
    {

        private formControlDescriptionsControlDescription controlDescriptionField;

        /// <remarks/>
        public formControlDescriptionsControlDescription controlDescription
        {
            get
            {
                return this.controlDescriptionField;
            }
            set
            {
                this.controlDescriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formControlDescriptionsControlDescription
    {

        private formControlDescriptionsControlDescriptionCustomControl[] customControlField;

        private string forControlField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("customControl")]
        public formControlDescriptionsControlDescriptionCustomControl[] customControl
        {
            get
            {
                return this.customControlField;
            }
            set
            {
                this.customControlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string forControl
        {
            get
            {
                return this.forControlField;
            }
            set
            {
                this.forControlField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formControlDescriptionsControlDescriptionCustomControl
    {

        private formControlDescriptionsControlDescriptionCustomControlParameters parametersField;

        private byte formFactorField;

        private string nameField;

        /// <remarks/>
        public formControlDescriptionsControlDescriptionCustomControlParameters parameters
        {
            get
            {
                return this.parametersField;
            }
            set
            {
                this.parametersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte formFactor
        {
            get
            {
                return this.formFactorField;
            }
            set
            {
                this.formFactorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formControlDescriptionsControlDescriptionCustomControlParameters
    {

        private formControlDescriptionsControlDescriptionCustomControlParametersDataset datasetField;

        private formControlDescriptionsControlDescriptionCustomControlParametersEntityTypeCode entityTypeCodeField;

        private formControlDescriptionsControlDescriptionCustomControlParametersLocation locationField;

        private formControlDescriptionsControlDescriptionCustomControlParametersMsinternalisvisibleinmocaonly msinternalisvisibleinmocaonlyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("data-set")]
        public formControlDescriptionsControlDescriptionCustomControlParametersDataset dataset
        {
            get
            {
                return this.datasetField;
            }
            set
            {
                this.datasetField = value;
            }
        }

        /// <remarks/>
        public formControlDescriptionsControlDescriptionCustomControlParametersEntityTypeCode EntityTypeCode
        {
            get
            {
                return this.entityTypeCodeField;
            }
            set
            {
                this.entityTypeCodeField = value;
            }
        }

        /// <remarks/>
        public formControlDescriptionsControlDescriptionCustomControlParametersLocation Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("msinternal.isvisibleinmocaonly")]
        public formControlDescriptionsControlDescriptionCustomControlParametersMsinternalisvisibleinmocaonly msinternalisvisibleinmocaonly
        {
            get
            {
                return this.msinternalisvisibleinmocaonlyField;
            }
            set
            {
                this.msinternalisvisibleinmocaonlyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formControlDescriptionsControlDescriptionCustomControlParametersDataset
    {

        private string viewIdField;

        private bool isUserViewField;

        private string targetEntityTypeField;

        private string filteredViewIdsField;

        private bool enableViewPickerField;

        private string nameField;

        /// <remarks/>
        public string ViewId
        {
            get
            {
                return this.viewIdField;
            }
            set
            {
                this.viewIdField = value;
            }
        }

        /// <remarks/>
        public bool IsUserView
        {
            get
            {
                return this.isUserViewField;
            }
            set
            {
                this.isUserViewField = value;
            }
        }

        /// <remarks/>
        public string TargetEntityType
        {
            get
            {
                return this.targetEntityTypeField;
            }
            set
            {
                this.targetEntityTypeField = value;
            }
        }

        /// <remarks/>
        public string FilteredViewIds
        {
            get
            {
                return this.filteredViewIdsField;
            }
            set
            {
                this.filteredViewIdsField = value;
            }
        }

        /// <remarks/>
        public bool EnableViewPicker
        {
            get
            {
                return this.enableViewPickerField;
            }
            set
            {
                this.enableViewPickerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formControlDescriptionsControlDescriptionCustomControlParametersEntityTypeCode
    {

        private string typeField;

        private bool staticField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool @static
        {
            get
            {
                return this.staticField;
            }
            set
            {
                this.staticField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formControlDescriptionsControlDescriptionCustomControlParametersLocation
    {

        private string typeField;

        private bool staticField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool @static
        {
            get
            {
                return this.staticField;
            }
            set
            {
                this.staticField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formControlDescriptionsControlDescriptionCustomControlParametersMsinternalisvisibleinmocaonly
    {

        private bool staticField;

        private bool valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool @static
        {
            get
            {
                return this.staticField;
            }
            set
            {
                this.staticField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public bool Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formDisplayConditions
    {

        private object everyoneField;

        private bool fallbackFormField;

        private byte orderField;

        /// <remarks/>
        public object Everyone
        {
            get
            {
                return this.everyoneField;
            }
            set
            {
                this.everyoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool FallbackForm
        {
            get
            {
                return this.fallbackFormField;
            }
            set
            {
                this.fallbackFormField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class formLibrary
    {

        private string nameField;

        private string libraryUniqueIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string libraryUniqueId
        {
            get
            {
                return this.libraryUniqueIdField;
            }
            set
            {
                this.libraryUniqueIdField = value;
            }
        }
    }


}
