using System;
using System.Linq;
using FluentAssertions;
using Opw.HttpExceptions.AspNetCore.Mappers;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class HttpExceptionsOptionsExtensionsTests
    {
        [Fact]
        public void ExceptionMapper_Should_Add2ExceptionMappers()
        {
            var options = new HttpExceptionsOptions();

            options.ExceptionMapper<Exception, ProblemDetailsExceptionMapper<Exception>>();
            options.ExceptionMapper<HttpException, ProblemDetailsExceptionMapper<HttpException>>();

            options.ExceptionMapperDescriptors.Should().HaveCount(2);
        }

        [Fact]
        public void ExceptionMapper_Should_OverrideExceptionMapper()
        {
            var options = new HttpExceptionsOptions();

            options.ExceptionMapper<Exception, ProblemDetailsExceptionMapper<Exception>>();
            options.ExceptionMapper<Exception, TestProblemDetailsExceptionMapper>();

            options.ExceptionMapperDescriptors.Should().HaveCount(1);
            options.ExceptionMapperDescriptors.First().Value.Type.Should().Be<TestProblemDetailsExceptionMapper>();
        }

        [Fact]
        public void HttpResponseMapper_Should_Add2HttpResponseMappers()
        {
            var options = new HttpExceptionsOptions();

            options.HttpResponseMapper<TestProblemDetailsHttpResponseMapper>(500);
            options.HttpResponseMapper<ProblemDetailsHttpResponseMapper>();

            options.HttpResponseMapperDescriptors.Should().HaveCount(2);
        }

        [Fact]
        public void HttpResponseMapper_Should_OverrideHttpResponseMapper()
        {
            var options = new HttpExceptionsOptions();

            options.HttpResponseMapper<ProblemDetailsHttpResponseMapper>(500);
            options.HttpResponseMapper<TestProblemDetailsHttpResponseMapper>(500);

            options.HttpResponseMapperDescriptors.Should().HaveCount(1);
            options.HttpResponseMapperDescriptors.First().Value.Type.Should().Be<TestProblemDetailsHttpResponseMapper>();
        }
    }
}
