using System;
namespace cuf_admision_domain.Exceptions
{
    public class BadStateChangeExeption : CufHttpException
    {
        public BadStateChangeExeption(string context, string message, string statusCode = "500", string traceInfo = "-.-")
            : base(context, message, statusCode, traceInfo)
        {
        }
    }
}

