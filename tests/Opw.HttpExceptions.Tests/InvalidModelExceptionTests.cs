using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Opw.HttpExceptions
{
    public class InvalidModelExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateInvalidModelException_With1Error()
        {
            var exception = new InvalidModelException("memberName", "error1", "error2");

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
            exception.Errors.Should().HaveCount(1);
        }

        [Fact]
        public void Constructor_Should_CreateInvalidModelException_With2Errors()
        {
            var errors = new Dictionary<string, string[]>();
            errors.Add("memberName1", new[] { "error1", "error2" });
            errors.Add("memberName2", new[] { "error1", "error2" });

            var exception = new InvalidModelException(errors);

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
            exception.Errors.Should().HaveCount(2);
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var errors = new Dictionary<string, string[]>();
            errors.Add("memberName1", new[] { "error1", "error2" });
            errors.Add("memberName2", new[] { "error1", "error2" });

            var exception = SerializationHelper.SerializeDeserialize(new InvalidModelException(errors));

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
            exception.Errors.Should().HaveCount(2);
        }
    }
}
