using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            ErrorCode = info.GetInt32("ErrorCode");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            base.GetObjectData(info, context);

            info.AddValue("ErrorCode", ErrorCode);
        }
    }
}
