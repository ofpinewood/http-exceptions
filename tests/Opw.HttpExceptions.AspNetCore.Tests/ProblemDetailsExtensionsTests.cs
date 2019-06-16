using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
            var mapper = TestsHelper.CreateExceptionMapper<Exception>(true);
            var problemDetails = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeTrue();
            exceptionDetails.Should().NotBeNull();
            exceptionDetails.Name.Should().Be(nameof(ApplicationException));
            exceptionDetails.Message.Should().NotBeNull();
        }

        [Fact]
        public void TryGetExceptionDetails_Should_ReturnFalse()
        {
            var mapper = TestsHelper.CreateExceptionMapper<Exception>(false);
            var problemDetails = mapper.Map(new ApplicationException(), new DefaultHttpContext());

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
