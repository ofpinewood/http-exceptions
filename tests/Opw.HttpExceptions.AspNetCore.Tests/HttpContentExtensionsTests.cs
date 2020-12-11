using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using Xunit;
using System.Net;

namespace Opw.HttpExceptions.AspNetCore
{
    public class HttpContentExtensionsTests
    {
        [Fact]
        public void ReadAsProblemDetails_Should_ReturnProblemDetails()
        {
            ApplicationException applicationException;
            try
            {
                throw new ApplicationException("Error!");
            }
            catch (ApplicationException ex)
            {
                applicationException = ex;
            }

            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(true);
            var actionResult = mapper.Map(applicationException, new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;
            var jsonContent = problemDetailsResult.Value.ToJsonContent("application/problem+json");

            var result = jsonContent.ReadAsProblemDetails();

            result.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            result.Title.Should().Be("Application");
            result.Detail.Should().Be("Error!");
            result.Type.Should().Be("error:application");
            result.Instance.Should().BeNull();

            var exception = result.ShouldHaveExceptionDetails();

            exception.Type.Should().Be("ApplicationException");
            exception.Message.Should().Be("Error!");
        }
    }
}
