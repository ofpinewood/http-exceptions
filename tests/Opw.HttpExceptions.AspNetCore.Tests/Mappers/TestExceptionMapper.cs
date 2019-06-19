using System;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    internal class TestExceptionMapper : ExceptionMapper<Exception>
    {
        public TestExceptionMapper(Microsoft.Extensions.Options.IOptions<HttpExceptionsOptions> options) : base(options) { }
    }
}
