using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore.Serialization
{
    public class ProblemDetailsExtensionsTests
    {
        [Fact]
        public void TryGetExceptionDetails_Should_ReturnSerializableException()
        {
            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(true);
            var actionResult = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            var result = problemDetailsResult.Value.TryGetExceptionDetails(out var exception);

            result.Should().BeTrue();
            exception.Should().NotBeNull();
            exception.Type.Should().Be(nameof(ApplicationException));
            exception.Message.Should().NotBeNull();
        }

        [Fact]
        public void TryGetExceptionDetails_Should_ReturnFalse()
        {
            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(false);
            var actionResult = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            var result = problemDetailsResult.Value.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeFalse();
            exceptionDetails.Should().BeNull();
        }

        [Fact]
        public void TryParseSerializableException_Should_ReturnTrue_ForTypeOfExceptionInfo()
        {
            var exception = new SerializableException(new ApplicationException());

            var result = exception.TryParseSerializableException(out var parsedException);

            result.Should().BeTrue();
            parsedException.Should().NotBeNull();
        }
    }
}
