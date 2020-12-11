using FluentAssertions;
using Moq;
using System;
using System.Net;
using System.Runtime.Serialization;
using Xunit;

namespace Opw.HttpExceptions
{
    public class HttpExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateHttpException_WithStatusCodeInternalServerError()
        {
            var exception = new HttpException();

            exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.InternalServerError);
        }

        [Fact]
        public void Constructor_Should_CreateHttpException_WithStatusCodeBadRequest()
        {
            var exception = new HttpException(HttpStatusCode.BadRequest);

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
        }

        [Fact]
        public void Constructor_Should_CreateHttpException_WithStatusCodeBadRequestAndMessage()
        {
            var exception = new HttpException(HttpStatusCode.BadRequest, "message");

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
            exception.Message.Should().Be("message");
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = SerializationHelper.SerializeDeserialize(new HttpException(HttpStatusCode.BadRequest));

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
        }

        [Fact]
        public void GetObjectData_Should_SerializeAndDeserialize()
        {
            var exception = new HttpException(HttpStatusCode.BadRequest);
            var serializationInfo = new SerializationInfo(typeof(HttpException), new Mock<IFormatterConverter>().Object);

            exception.GetObjectData(serializationInfo, new StreamingContext());

            var result = serializationInfo.GetValue("StatusCode", typeof(HttpStatusCode));
            result.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void GetObjectData_Should_ThrowArgumentNullException_WhenSerializationInfoNull()
        {
            var exception = new HttpException(HttpStatusCode.BadRequest);
            var serializationInfo = new SerializationInfo(typeof(HttpException), new Mock<IFormatterConverter>().Object);

            Action action = () => exception.GetObjectData(null, new StreamingContext());

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
