using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Opw.HttpExceptions.AspNetCore.Mappers;
using System;

namespace Opw.HttpExceptions.AspNetCore.Sample.CustomErrors
{
    // Using the ProblemDetailsExceptionMapper as a base so we don't have to implement all the mapping.
    public class CustomExceptionMapper : ProblemDetailsExceptionMapper<FormatException>
    {
        public CustomExceptionMapper(Microsoft.Extensions.Options.IOptions<HttpExceptionsOptions> options) : base(options)
        {
        }

        public override IStatusCodeActionResult Map(Exception exception, HttpContext context)
        {
            if (!(exception is FormatException ex))
                throw new ArgumentOutOfRangeException(nameof(exception), exception, "Exception is not of type FormatException.");

            var customError = new CustomError
            {
                Status = 418,
                Code = 42, // some code identifying the error
                Type = MapType(ex, context),
                Message = MapDetail(ex, context),
                TraceId = Guid.NewGuid().ToString()
            };

            return new CustomErrorResult(customError);
        }
    }
}
