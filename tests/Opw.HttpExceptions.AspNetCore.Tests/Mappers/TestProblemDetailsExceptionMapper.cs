using System;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    internal class TestProblemDetailsExceptionMapper : ProblemDetailsExceptionMapper<Exception>
    {
        public TestProblemDetailsExceptionMapper(Microsoft.Extensions.Options.IOptions<HttpExceptionsOptions> options) : base(options) { }
    }
}
