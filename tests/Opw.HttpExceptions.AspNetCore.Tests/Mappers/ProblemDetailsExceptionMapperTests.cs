using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Opw.HttpExceptions.AspNetCore.Tests.TestData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    public class ProblemDetailsExceptionMapperTests
    {
        private readonly ExposeProtectedProblemDetailsExceptionMapper _mapper;

        public ProblemDetailsExceptionMapperTests()
        {
            var optionsMock = TestHelper.CreateHttpExceptionsOptionsMock(false);
            _mapper = new ExposeProtectedProblemDetailsExceptionMapper(optionsMock.Object);
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails()
        {
            var actionResult = _mapper.Map(new ApplicationException(), new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            problemDetailsResult.Value.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            problemDetailsResult.Value.Instance.Should().BeNull();

            var result = problemDetailsResult.Value.TryGetExceptionDetails(out var exception);

            result.Should().BeFalse();
            exception.Should().BeNull();
        }

        [Fact]
        public void Map_Should_ReturnThrowArgumentOutOfRangeException_ForInvalidExceptionType()
        {
            var mapper = new ProblemDetailsExceptionMapper<ApplicationException>(TestHelper.CreateHttpExceptionsOptionsMock(false).Object);

            Action action = () => mapper.Map(new NotSupportedException(), new DefaultHttpContext());

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails_WithHelpLink()
        {
            var helpLink = "https://docs.microsoft.com/en-us/dotnet/api/system.exception.helplink?view=netcore-2.2";
            var actionResult = _mapper.Map(new ApplicationException { HelpLink = helpLink }, new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            problemDetailsResult.Value.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            problemDetailsResult.Value.Instance.Should().Be(helpLink);

            var result = problemDetailsResult.Value.TryGetExceptionDetails(out var exception);

            result.Should().BeFalse();
            exception.Should().BeNull();
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails_WithExceptionDetails()
        {
            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(true);
            var actionResult = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            problemDetailsResult.Value.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            problemDetailsResult.Value.Instance.Should().BeNull();

            var result = problemDetailsResult.Value.TryGetExceptionDetails(out var exception);

            result.Should().BeTrue();
            exception.Should().NotBeNull();
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails_WithValidationErrors_ForValidationErrorException()
        {
            var exception = new ValidationErrorException<string>("param", new[] { "error1", "error1" });

            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(true);
            var actionResult = mapper.Map(exception, new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            problemDetailsResult.Value.ShouldNotBeNull(HttpStatusCode.BadRequest);
            problemDetailsResult.Value.Instance.Should().Be(ResponseStatusCodeLink.BadRequest);

            var result = problemDetailsResult.Value.TryGetErrors(out var errors);

            result.Should().BeTrue();
            errors.Should().NotBeNull();
            errors.Should().HaveCount(1);
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails_WithValidationErrors2_ForValidationErrorException()
        {
            var errorsIn = new Dictionary<string, string[]>();
            errorsIn.Add("memberName1", new[] { "error1", "error2" });
            errorsIn.Add("memberName2", new[] { "error1", "error2" });
            var exception = new ValidationErrorException<string>(errorsIn);

            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(true);
            var actionResult = mapper.Map(exception, new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            problemDetailsResult.Value.ShouldNotBeNull(HttpStatusCode.BadRequest);
            problemDetailsResult.Value.Instance.Should().Be(ResponseStatusCodeLink.BadRequest);

            var result = problemDetailsResult.Value.TryGetErrors(out var errors);

            result.Should().BeTrue();
            errors.Should().NotBeNull();
            errors.Should().HaveCount(2);
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails_WithValidationErrors2_ForValidationErrorException_WithComplexErrorObject()
        {
            var errorsIn = new Dictionary<string, ValidationResult[]>();
            errorsIn.Add("memberName1", new[] { new ValidationResult("message", new[] { "memberName1" }) });
            errorsIn.Add("memberName2", new[] { new ValidationResult("message", new[] { "memberName2" }) });
            var exception = new ValidationErrorException<ValidationResult>(errorsIn);

            var mapper = TestHelper.CreateProblemDetailsExceptionMapper<Exception>(true);
            var actionResult = mapper.Map(exception, new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            problemDetailsResult.Value.ShouldNotBeNull(HttpStatusCode.BadRequest);
            problemDetailsResult.Value.Instance.Should().Be(ResponseStatusCodeLink.BadRequest);

            var result = problemDetailsResult.Value.TryGetErrors(out var errors);

            result.Should().BeTrue();
            errors.Should().NotBeNull();
            errors.Should().HaveCount(2);
        }

        [Fact]
        public void MapDetail_Should_ReturnExceptionMessage()
        {
            var message = "Test exception message.";
            var exception = new ApplicationException(message);
            var result = _mapper.MapDetail(exception, new DefaultHttpContext());

            result.Should().Be(message);
        }

        [Fact]
        public void MapInstance_Should_ReturnExceptionHelpLink()
        {
            var helpLink = "https://docs.microsoft.com/en-us/dotnet/api/system.exception.helplink?view=netcore-2.2";
            var exception = new ApplicationException { HelpLink = helpLink };
            var result = _mapper.MapInstance(exception, new DefaultHttpContext());

            result.Should().Be(helpLink);
        }

        [Fact]
        public void MapInstance_Should_ReturnRequestPath()
        {
            var exception = new ApplicationException();
            var context = new DefaultHttpContext();
            var requestPath = "/test/123";
            context.Request.Path = requestPath;
            var result = _mapper.MapInstance(exception, context);

            result.Should().Be(requestPath);
        }

        [Fact]
        public void MapStatus_Should_ReturnInternalServerError()
        {
            var exception = new ApplicationException();
            var result = _mapper.MapStatus(exception, new DefaultHttpContext());

            result.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void MapStatus_Should_ReturnBadRequest()
        {
            var exception = new HttpException(HttpStatusCode.BadRequest);
            var result = _mapper.MapStatus(exception, new DefaultHttpContext());

            result.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails_With_Attributed_Properties()
        {
            var actionResult = _mapper.Map(new MyBadRequestException("UnitTest") { Foo = "FooUnit", Bar = 123, FooBar = 42L }, new DefaultHttpContext());

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            problemDetailsResult.Value.ShouldNotBeNull(HttpStatusCode.BadRequest);
            problemDetailsResult.Value.Instance.Should().NotBeNull();

            problemDetailsResult.Value.Extensions.Should().ContainKey("foo");
            problemDetailsResult.Value.Extensions.Should().ContainKey("bar");
            problemDetailsResult.Value.Extensions.Should().NotContainKey("fooBar");
            problemDetailsResult.Value.Extensions.Should().ContainValue("FooUnit");
            problemDetailsResult.Value.Extensions.Should().ContainValue(123);
            problemDetailsResult.Value.Extensions.Should().NotContainValue(42L);

            var result = problemDetailsResult.Value.TryGetExceptionDetails(out var exception);

            result.Should().BeFalse();
            exception.Should().BeNull();
        }

        [Fact]
        public void MapTitle_Should_ReturnFormattedExceptionName()
        {
            var exception = new DivideByZeroException();
            var result = _mapper.MapTitle(exception, new DefaultHttpContext());

            result.Should().Be("DivideByZero");
        }

        [Fact]
        public void MapType_Should_ReturnFormattedExceptionName()
        {
            var exception = new DivideByZeroException();
            var result = _mapper.MapType(exception, new DefaultHttpContext());

            result.Should().Be("error:divide-by-zero");
        }

        [Fact]
        public void MapType_Should_ReturnExceptionHelpLink_WhenUseHelpLinkAsProblemDetailsTypeTrue()
        {
            var options = new HttpExceptionsOptions
            {
                UseHelpLinkAsProblemDetailsType = true,
                DefaultHelpLink = new Uri("http://www.example.com/help-page")
            };
            var optionsMock = new Mock<IOptions<HttpExceptionsOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var mapper = new ExposeProtectedProblemDetailsExceptionMapper(optionsMock.Object);
            var helpLink = "https://docs.microsoft.com/en-us/dotnet/api/system.exception.helplink?view=netcore-2.2";
            var exception = new ApplicationException { HelpLink = helpLink };

            var result = mapper.MapType(exception, new DefaultHttpContext());

            result.Should().Be(helpLink);
        }

        [Fact]
        public void MapType_Should_ReturnDefaultHelpLink_WhenUseHelpLinkAsProblemDetailsTypeTrue()
        {
            var options = new HttpExceptionsOptions
            {
                UseHelpLinkAsProblemDetailsType = true,
                DefaultHelpLink = new Uri("http://www.example.com/help-page")
            };
            var optionsMock = new Mock<IOptions<HttpExceptionsOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var mapper = new ExposeProtectedProblemDetailsExceptionMapper(optionsMock.Object);
            var exception = new ApplicationException();

            var result = mapper.MapType(exception, new DefaultHttpContext());

            result.Should().Be("http://www.example.com/help-page");
        }

        [Fact]
        public void MapType_Should_ReturnTypeAsErrorUri_WhenUseHelpLinkAsProblemDetailsTypeTrueAndDefaultHelpLinkForProblemDetailsTypeNull()
        {
            var options = new HttpExceptionsOptions
            {
                UseHelpLinkAsProblemDetailsType = true
            };
            var optionsMock = new Mock<IOptions<HttpExceptionsOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var mapper = new ExposeProtectedProblemDetailsExceptionMapper(optionsMock.Object);
            var exception = new ApplicationException();

            var result = mapper.MapType(exception, new DefaultHttpContext());

            result.Should().Be("error:application");
        }

        [Fact]
        public void MapType_Should_ReturnExceptionHelpLinkForHttpException_WhenUseHelpLinkAsProblemDetailsTypeTrue()
        {
            var options = new HttpExceptionsOptions
            {
                UseHelpLinkAsProblemDetailsType = true,
                DefaultHelpLink = new Uri("http://www.example.com/default")
            };

            var optionsMock = new Mock<IOptions<HttpExceptionsOptions>>();
            optionsMock.Setup(o => o.Value).Returns(options);

            var mapper = new ExposeProtectedProblemDetailsExceptionMapper(optionsMock.Object);
            var exception = new BadRequestException();

            var result = mapper.MapType(exception, new DefaultHttpContext());

            result.Should().Be(exception.HelpLink);
        }

        private class ExposeProtectedProblemDetailsExceptionMapper : ProblemDetailsExceptionMapper<Exception>
        {
            public ExposeProtectedProblemDetailsExceptionMapper(IOptions<HttpExceptionsOptions> options) : base(options) { }

            public new string MapDetail(Exception exception, HttpContext context)
            {
                return base.MapDetail(exception, context);
            }

            public new string MapInstance(Exception exception, HttpContext context)
            {
                return base.MapInstance(exception, context);
            }

            public new int MapStatus(Exception exception, HttpContext context)
            {
                return base.MapStatus(exception, context);
            }

            public new string MapTitle(Exception exception, HttpContext context)
            {
                return base.MapTitle(exception, context);
            }

            public new string MapType(Exception exception, HttpContext context)
            {
                return base.MapType(exception, context);
            }
        }
    }
}
