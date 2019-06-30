using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class ProblemDetailsResultTests
    {
        [Fact]
        public void Constructor_Should_ConstructProblemDetailsResult()
        {
            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(true);
            var problemDetails = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            var problemDetailsResult = new ProblemDetailsResult(problemDetails);

            problemDetailsResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            problemDetailsResult.DeclaredType.Should().Be(typeof(ProblemDetails));
            problemDetailsResult.ContentTypes.Should().HaveCount(2);
            problemDetailsResult.ContentTypes.Should().Contain("application/problem+json");
            problemDetailsResult.Value.Should().NotBeNull();
            problemDetailsResult.Value.Should().BeOfType<ProblemDetails>();
        }
    }
}
