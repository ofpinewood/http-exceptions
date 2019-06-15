using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class ProblemDetailsExtensionsTests
    {
        [Fact]
        public void TryGetExceptionDetails_Should_ReturnExceptionDetails()
        {
            var problemDetails = new ApplicationException().ToProblemDetails(true);

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeTrue();
            exceptionDetails.Should().NotBeNull();
            exceptionDetails.Name.Should().Be(nameof(ApplicationException));
            exceptionDetails.Message.Should().NotBeNull();
        }

        [Fact]
        public void TryGetExceptionDetails_Should_ReturnFalse()
        {
            var problemDetails = new ApplicationException().ToProblemDetails(false);

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeFalse();
            exceptionDetails.Should().BeNull();
        }

        [Fact]
        public void TryParseExceptionDetails_Should_ReturnTrue_ForTypeOfExceptionDetails()
        {
            var exceptionDetails = new ExceptionDetails(new ApplicationException());

            var result = exceptionDetails.TryParseExceptionDetails(out var parsedExceptionDetails);

            result.Should().BeTrue();
            parsedExceptionDetails.Should().NotBeNull();
        }

        [Fact]
        public void TryParseExceptionDetails_Should_ReturnTrue_ForTypeOfJToken()
        {
            var exceptionDetails = JToken.FromObject(new ExceptionDetails(new ApplicationException()));

            var result = exceptionDetails.TryParseExceptionDetails(out var parsedExceptionDetails);

            result.Should().BeTrue();
            parsedExceptionDetails.Should().NotBeNull();
        }
    }
}
