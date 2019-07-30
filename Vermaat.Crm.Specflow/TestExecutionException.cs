using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow
{

    [Serializable]
    public class TestExecutionException : Exception
    {
        public virtual int ErrorCode { get;  }
        public TestExecutionException() { }

        public TestExecutionException(int errorCode) : 
            base(GlobalTestingContext.ErrorCodes.GetErrorMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        public TestExecutionException(int errorCode, params object[] formatArgs) :
            base(GlobalTestingContext.ErrorCodes.GetErrorMessage(errorCode, formatArgs))
        {
            ErrorCode = errorCode;
        }

        public TestExecutionException(int errorCode, Exception inner) : 
            base(GlobalTestingContext.ErrorCodes.GetErrorMessage(errorCode), inner)
        {
            ErrorCode = errorCode;
        }

        public TestExecutionException(int errorCode, Exception inner, params object[] formatArgs) :
           base(GlobalTestingContext.ErrorCodes.GetErrorMessage(errorCode, formatArgs), inner)
        {
            ErrorCode = errorCode;
        }

        protected TestExecutionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
