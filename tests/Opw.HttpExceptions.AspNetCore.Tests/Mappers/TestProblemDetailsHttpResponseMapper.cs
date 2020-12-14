using System.Diagnostics.CodeAnalysis;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    [SuppressMessage("Style", "CA1812:Avoid uninstantiated internal classes", Justification = "Used with the test HttpExceptionsOptions.")]
    internal class TestProblemDetailsHttpResponseMapper : ProblemDetailsHttpResponseMapper
    {
        public TestProblemDetailsHttpResponseMapper(Microsoft.Extensions.Options.IOptions<HttpExceptionsOptions> options) : base(options) { }
    }
}
