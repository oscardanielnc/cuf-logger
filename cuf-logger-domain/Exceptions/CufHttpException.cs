using System;
namespace cuf_admision_domain.Exceptions
{
    public class CufHttpException : Exception
    {
        public string statusCode { get; set; }
        public string context { get; set; }
        public string message { get; set; }
        public string traceInfo { get; set; }

        public CufHttpException(string context, string message, string statusCode = "500", string traceInfo = "-.-")
            : base($"[{context}] Error: {statusCode} -> {message} \n -> Other: {traceInfo}")
        {
            this.statusCode = statusCode;
            this.context = context;
            this.message = message;
            this.traceInfo = traceInfo;
        }
    }
}

