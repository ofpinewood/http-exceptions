using System;
using System.Net;
using System.Runtime.Serialization;

namespace Opw.HttpExceptions.AspNetCore.Sample.CustomErrors
{
    public class ProblemDetailsAttributeException : HttpException
    {
        public ProblemDetailsAttributeException(string message) : base(HttpStatusCode.Ambiguous, message) { }

        public ProblemDetailsAttributeException(string message, Exception innerException) : base(message, innerException) { }

        public ProblemDetailsAttributeException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [ProblemDetails]
        public string PropertyA { get; set; }

        [ProblemDetails]
        public int PropertyB { get; set; }

        public long PropertyC { get; set; }
    }
}
