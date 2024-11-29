using System;
namespace cuf_admision_domain.Exceptions
{
    public class CustomException : Exception
    {
        public int _httpStatusCode { get; set; }
        public int _errorCode { get; set; }
        public CustomException(int httpStatusCode, int errorCode, string message)
            : base($"{errorCode} {message}")
        {
            _httpStatusCode = httpStatusCode;
            _errorCode = errorCode;
        }
    }
}

