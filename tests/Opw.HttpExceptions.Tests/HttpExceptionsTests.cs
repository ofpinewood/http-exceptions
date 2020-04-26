using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using Xunit;

namespace Opw.HttpExceptions
{
    public class HttpExceptionsTests
    {
        public static IEnumerable<object[]> HttpExceptionTypes => new List<object[]>
        {
            new object[] { typeof(BadRequestException) },
            new object[] { typeof(ConflictException) },
            new object[] { typeof(ForbiddenException) },
            new object[] { typeof(HttpException) },
            new object[] { typeof(InvalidModelException) },
            new object[] { typeof(NotFoundException) },
            new object[] { typeof(NotFoundException<object>) },
            new object[] { typeof(ServiceUnavailableException) },
            new object[] { typeof(UnauthorizedException) },
            new object[] { typeof(UnsupportedMediaTypeException) },
            new object[] { typeof(ValidationErrorException<object>) },
        };

        [Theory]
        [MemberData(nameof(HttpExceptionTypes))]
        public void Constructor_Should_HaveNoParameters(Type httpExceptionType)
        {
            // the following types don't have parameterless constructors
            if (httpExceptionType == typeof(InvalidModelException)) return;
            if (httpExceptionType == typeof(ValidationErrorException<object>)) return;

            var httpException = Activator.CreateInstance(httpExceptionType);

            httpException.Should().NotBeNull();
            httpException.Should().BeOfType(httpExceptionType);
        }

        [Theory]
        [MemberData(nameof(HttpExceptionTypes))]
        public void Constructor_Should_HaveStringParameters(Type httpExceptionType)
        {
            // the following types don't have this type of constructor
            if (httpExceptionType == typeof(InvalidModelException)) return;

            var httpException = Activator.CreateInstance(httpExceptionType, "message");

            httpException.Should().NotBeNull();
            httpException.Should().BeOfType(httpExceptionType);
        }

        [Theory]
        [MemberData(nameof(HttpExceptionTypes))]
        public void Constructor_Should_HaveStringAndExceptionParameters(Type httpExceptionType)
        {
            // the following types don't have this type of constructor
            if (httpExceptionType == typeof(InvalidModelException)) return;

            var httpException = Activator.CreateInstance(httpExceptionType, "message", new ArgumentException());

            httpException.Should().NotBeNull();
            httpException.Should().BeOfType(httpExceptionType);
        }

        [Theory]
        [MemberData(nameof(HttpExceptionTypes))]
        public void Serialization_Should_SerializeAndDeserialize(Type httpExceptionType)
        {
            // specific test are available for the following types
            if (httpExceptionType == typeof(InvalidModelException)) return;
            if (httpExceptionType == typeof(ValidationErrorException<object>)) return;

            var httpException = (HttpExceptionBase)Activator.CreateInstance(httpExceptionType);

            var exception = (HttpExceptionBase)SerializationHelper.SerializeDeserialize(httpException);

            exception.StatusCode.Should().Be(httpException.StatusCode);
            exception.HelpLink.Should().Be(httpException.HelpLink);
        }
    }
}
