using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using static Vermaat.Crm.Specflow.TableConverter;

namespace Vermaat.Crm.Specflow.Expressions
{
    [Binding]
    public class FormulaHooks
    {
        private readonly CrmTestingContext _crmContext;
        private readonly FormulaContext _formulaContext;

        public FormulaHooks(CrmTestingContext crmContext, FormulaContext formulaContext)
        {
            _crmContext = crmContext;
            _formulaContext = formulaContext;
        }

        [BeforeScenario]
        public void PrepareParsing()
        {
            _crmContext.TableConverter.OnRowProcessed += ParseRow;
        }

        private void ParseRow(object sender, TableRowEventArgs e)
        {
            if (!e.Row.ContainsKey(Constants.SpecFlow.TABLE_VALUE))
                return;

            var value = e.Row[Constants.SpecFlow.TABLE_VALUE];

            if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("="))
                return;

            object parsed = _formulaContext.Parse(value.Substring(1));
            var attribute = GlobalTestingContext.Metadata.GetAttributeMetadata(e.EntityName, e.Row[Constants.SpecFlow.TABLE_KEY]);
            e.Row[Constants.SpecFlow.TABLE_VALUE] = ObjectConverter.ToSpecflowTableValue(attribute, parsed);


        }
    }
}
