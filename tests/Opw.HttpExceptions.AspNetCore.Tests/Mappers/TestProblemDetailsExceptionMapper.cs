using System;
using System.Diagnostics.CodeAnalysis;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    [SuppressMessage("Style", "CA1812:Avoid uninstantiated internal classes", Justification = "Used with the test HttpExceptionsOptions.")]
    internal class TestProblemDetailsExceptionMapper : ProblemDetailsExceptionMapper<Exception>
    {
        public TestProblemDetailsExceptionMapper(Microsoft.Extensions.Options.IOptions<HttpExceptionsOptions> options) : base(options) { }
    }
}
