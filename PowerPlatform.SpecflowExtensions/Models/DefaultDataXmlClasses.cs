using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PowerPlatform.SpecflowExtensions.Models
{
    [Serializable()]
    [XmlType(AnonymousType = true)]
    [XmlRoot("defaultdata")]
    public partial class DefaultData
    {
        [XmlElement("entity")]
        public DefaultDataEntity[] Entities { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public class DefaultDataEntity
    {
        [XmlElement("field")]
        public DefaultDataField[] Fields { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    /// <remarks/>
    [Serializable()]
    [XmlType(AnonymousType = true)]
    public class DefaultDataField
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText()]
        public string Value { get; set; }
    }
}
