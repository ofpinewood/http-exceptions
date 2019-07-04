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
            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(true);
            var actionResult = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            var result = problemDetailsResult.Value.TryGetException<Exception>(out var exception);

            result.Should().BeTrue();
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ApplicationException>();
            exception.Message.Should().NotBeNull();
        }

        [Fact]
        public void TryGetExceptionDetails_Should_ReturnFalse()
        {
            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(false);
            var actionResult = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            var result = problemDetailsResult.Value.TryGetException<Exception>(out var exception);

            result.Should().BeFalse();
            exception.Should().BeNull();
        }

        [Fact]
        public void TryParseExceptionDetails_Should_ReturnTrue_ForTypeOfExceptionDetails()
        {
            var exception = new ApplicationException("Error!", new ApplicationException());

            var result = exception.TryParseException<Exception>(out var parsedException);

            result.Should().BeTrue();
            parsedException.Should().NotBeNull();
        }

        [Fact]
        public void TryParseExceptionDetails_Should_ReturnTrue_ForTypeOfJToken()
        {
            var exception = JToken.FromObject(new ApplicationException("Error!", new ApplicationException()));

            var result = exception.TryParseException<Exception>(out var parsedException);

            result.Should().BeTrue();
            parsedException.Should().NotBeNull();
        }
    }
}
