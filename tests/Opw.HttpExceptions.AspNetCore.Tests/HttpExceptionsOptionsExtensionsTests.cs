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

            options.ExceptionMapper<Exception, ExceptionMapper<Exception>>();
            options.ExceptionMapper<HttpException, ExceptionMapper<HttpException>>();

            options.ExceptionMapperDescriptors.Should().HaveCount(2);
        }

        [Fact]
        public void ExceptionMapper_Should_OverrideExceptionMapper()
        {
            var options = new HttpExceptionsOptions();

            options.ExceptionMapper<Exception, ExceptionMapper<Exception>>();
            options.ExceptionMapper<Exception, TestExceptionMapper>();

            options.ExceptionMapperDescriptors.Should().HaveCount(1);
            options.ExceptionMapperDescriptors.First().Value.Type.Should().Be<TestExceptionMapper>();
        }

        [Fact]
        public void HttpResponseMapper_Should_Add2HttpResponseMappers()
        {
            var options = new HttpExceptionsOptions();

            options.HttpResponseMapper<TestHttpResponseMapper>(500);
            options.HttpResponseMapper<HttpResponseMapper>();

            options.HttpResponseMapperDescriptors.Should().HaveCount(2);
        }

        [Fact]
        public void HttpResponseMapper_Should_OverrideHttpResponseMapper()
        {
            var options = new HttpExceptionsOptions();

            options.HttpResponseMapper<HttpResponseMapper>(500);
            options.HttpResponseMapper<TestHttpResponseMapper>(500);

            options.HttpResponseMapperDescriptors.Should().HaveCount(1);
            options.HttpResponseMapperDescriptors.First().Value.Type.Should().Be<TestHttpResponseMapper>();
        }
    }
}
