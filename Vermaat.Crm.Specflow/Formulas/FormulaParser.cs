using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Expressions;

namespace Vermaat.Crm.Specflow.Formulas
{
    public class FormulaParser
    {
        private static readonly string _datetimeFormat;
        private static readonly string _dateonlyFormat;

        public SpecflowFormulas FormulaHolder { get; set; }
        

        static FormulaParser()
        {
            _dateonlyFormat = HelperMethods.GetAppSettingsValue("DateFormat", false);
            _datetimeFormat = HelperMethods.GetAppSettingsValue("DateTimeFormat", false);
        }

        public FormulaParser()
        {
            FormulaHolder = new SpecflowFormulas();
        }

        public FormulaParser(SpecflowFormulas formulas)
        {
            FormulaHolder = formulas;
        }

        public string ParseFormula(AttributeMetadata attribute, string parseString)
        {
            switch (attribute.AttributeType)
            {
                case AttributeTypeCode.Boolean:
                    return Parse<bool>(parseString).ToString();

                case AttributeTypeCode.Double:
                    return Parse<double>(parseString).ToString();

                case AttributeTypeCode.Decimal:
                case AttributeTypeCode.Money:
                    return Parse<decimal>(parseString).ToString();

                case AttributeTypeCode.Integer:
                    return Parse<int>(parseString).ToString();

                case AttributeTypeCode.DateTime:
                    return ParseDateTime(attribute, parseString);

                case AttributeTypeCode.Memo:
                case AttributeTypeCode.String:
                case AttributeTypeCode.Picklist:
                case AttributeTypeCode.State:
                case AttributeTypeCode.Status:
                case AttributeTypeCode.Customer:
                case AttributeTypeCode.Lookup:
                case AttributeTypeCode.Owner:
                    return GlobalTestingContext.FormulaParser.Parse<string>(parseString).ToString();

                default: throw new NotImplementedException(string.Format("Type {0} not implemented", attribute.AttributeType));
            }
        }

        private string ParseDateTime(AttributeMetadata attribute, string parseString)
        {
            var dateTime = Parse<DateTime>(parseString);
            var format = ((DateTimeAttributeMetadata)attribute).Format == DateTimeFormat.DateOnly ?
                _dateonlyFormat : _datetimeFormat;

            return dateTime.ToString(format);
        }

        private T Parse<T>(string parseString)
        {
            var compiled = Eval.Compile(parseString, FormulaHolder.GetType());
            var result = compiled(FormulaHolder);
            return (T)Convert.ChangeType(result, typeof(T));
        }


    }
}
