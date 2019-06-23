namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    internal class TestHttpResponseMapper : HttpResponseMapper
    {
        public TestHttpResponseMapper(Microsoft.Extensions.Options.IOptions<HttpExceptionsOptions> options) : base(options) { }
    }
}
