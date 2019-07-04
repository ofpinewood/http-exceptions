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
        public void TryGetExceptionInfo_Should_ReturnExceptionInfo()
        {
            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(true);
            var actionResult = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            var result = problemDetailsResult.Value.TryGetExceptionInfo(out var exceptionInfo);

            result.Should().BeTrue();
            exceptionInfo.Should().NotBeNull();
            exceptionInfo.Type.Should().Be(nameof(ApplicationException));
            exceptionInfo.Message.Should().NotBeNull();
        }

        [Fact]
        public void TryGetExceptionInfo_Should_ReturnFalse()
        {
            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(false);
            var actionResult = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            var result = problemDetailsResult.Value.TryGetExceptionInfo(out var exceptionInfo);

            result.Should().BeFalse();
            exceptionInfo.Should().BeNull();
        }

        [Fact]
        public void TryParseExceptionInfo_Should_ReturnTrue_ForTypeOfExceptionInfo()
        {
            var exception = new ExceptionInfo(new ApplicationException());

            var result = exception.TryParseExceptionInfo(out var parsedExceptionInfo);

            result.Should().BeTrue();
            parsedExceptionInfo.Should().NotBeNull();
        }

        [Fact]
        public void TryParseExceptionInfo_Should_ReturnTrue_ForTypeOfJToken()
        {
            var exception = JToken.FromObject(new ExceptionInfo(new ApplicationException()));

            var result = exception.TryParseExceptionInfo(out var parsedExceptionInfo);

            result.Should().BeTrue();
            parsedExceptionInfo.Should().NotBeNull();
        }
    }
}
