using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Opw.HttpExceptions.AspNetCore.Sample
{
    public static class ProblemDetailsExtensions
    {
        public static ExceptionDetails ShouldHaveExceptionDetails(this ProblemDetails problemDetails)
        {
            problemDetails.TryGetExceptionDetails(out var exceptionDetails).Should().BeTrue();

            exceptionDetails.Should().NotBeNull();
            exceptionDetails.Name.Should().NotBeNull();
            exceptionDetails.Source.Should().NotBeNull();
            exceptionDetails.StackTrace.Should().NotBeNull();

            return exceptionDetails;
        }
    }
}
