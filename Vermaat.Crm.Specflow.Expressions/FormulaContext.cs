using Flee.PublicTypes;
using System;
using System.Globalization;

namespace Vermaat.Crm.Specflow.Expressions
{
    public class FormulaContext
    {
        private readonly ExpressionContext _expressionContext;

        public FormulaContext()
        {
            _expressionContext = new ExpressionContext();
            _expressionContext.Imports.AddType(typeof(FormulaFunctions));
            _expressionContext.Options.ParseCulture = CultureInfo.InvariantCulture;
        }

        public object Parse(string expression)
        {
            return _expressionContext.CompileDynamic(expression).Evaluate();
        }

        public T Parse<T>(string expression)
        {
            return _expressionContext.CompileGeneric<T>(expression).Evaluate();
        }

        public void AddFunctionType(Type type)
        {
            _expressionContext.Imports.AddType(type);
        }


    }
}
