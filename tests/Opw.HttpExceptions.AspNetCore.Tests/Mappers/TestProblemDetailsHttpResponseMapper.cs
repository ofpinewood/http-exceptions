namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    internal class TestProblemDetailsHttpResponseMapper : ProblemDetailsHttpResponseMapper
    {
        public TestProblemDetailsHttpResponseMapper(Microsoft.Extensions.Options.IOptions<HttpExceptionsOptions> options) : base(options) { }
    }
}
