using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{
    public class ValidationResult
    {
        private readonly List<string> _errors;

        public ValidationResult()
        {
            _errors = new List<string>();
        }

        public void AddError(string message)
        {
            _errors.Add(message);
        }

        public bool IsValid => _errors.Count == 0;
        public IReadOnlyCollection<string> Errors => _errors;
    }
}
