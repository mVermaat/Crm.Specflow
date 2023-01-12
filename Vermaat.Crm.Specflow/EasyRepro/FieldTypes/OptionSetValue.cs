﻿using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro.FieldTypes
{
    public class OptionSetValue
    {
        public OptionSetValue(int? value, string label)
        {
            Value = value;
            Label = label;
        }

        public OptionSet ToOptionSet(string logicalName)
        {
            return new OptionSet { Name = logicalName, Value = Value.HasValue ? Label : "-1" };
        }

        public int? Value { get; }
        public string Label { get; }
    }
}
