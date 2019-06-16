using System;
using System.Linq;
using FluentAssertions;
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

        private class TestExceptionMapper : ExceptionMapper<Exception>
        {
            public TestExceptionMapper(Microsoft.Extensions.Options.IOptions<HttpExceptionsOptions> options) : base(options) { }
        }
    }
}
