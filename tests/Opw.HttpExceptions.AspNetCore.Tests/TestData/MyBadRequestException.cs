using Opw.HttpExceptions.Attributes;
using System;
using System.Runtime.Serialization;

namespace Opw.HttpExceptions.AspNetCore.Tests.TestData
{
    public class MyBadRequestException : BadRequestException
    {
        public MyBadRequestException(string message) : base(message) { }

        public MyBadRequestException(string message, Exception innerException) : base(message, innerException) { }

        public MyBadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [ProblemDetails]
        public string Foo { get; set; }

        [ProblemDetails]
        public int Bar { get; set; }

        public long FooBar { get; set; }
    }
}
