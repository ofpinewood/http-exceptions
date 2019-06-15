using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class ExceptionExtensionsTests
    {
        [Fact]
        public void ToProblemDetails_Should_ReturnProblemDetails()
        {
            var problemDetails = new ApplicationException().ToProblemDetails(false);

            problemDetails.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            problemDetails.Instance.Should().BeNull();

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeFalse();
            exceptionDetails.Should().BeNull();
        }

        [Fact]
        public void ToProblemDetails_Should_ReturnProblemDetails_WithHelpLink()
        {
            var helpLink = "https://docs.microsoft.com/en-us/dotnet/api/system.exception.helplink?view=netcore-2.2";
            var problemDetails = new ApplicationException { HelpLink = helpLink }.ToProblemDetails(false);

            problemDetails.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            problemDetails.Instance.Should().Be(helpLink);

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeFalse();
            exceptionDetails.Should().BeNull();
        }

        [Fact]
        public void ToProblemDetails_Should_ReturnProblemDetails_WithExceptionDetails()
        {
            var problemDetails = new ApplicationException().ToProblemDetails(true);

            problemDetails.ShouldNotBeNull(HttpStatusCode.InternalServerError);

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeTrue();
            exceptionDetails.Should().NotBeNull();
        }
    }
}
