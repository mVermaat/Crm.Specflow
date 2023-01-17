using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    internal class FormCellParsingContext
    {
        

        public Dictionary<string, AttributeMetadata> MetadataDic { get; set; }
        public UCIApp App { get; set; }
        public FormCell Cell { get; set; }
        public bool IsHeader { get; set; }
        public SystemFormType FormType { get; set; }

        public string SectionName { get; set; }
        public string TabName { get; set; }
        public string TabLabel { get; set; }

    }
}
