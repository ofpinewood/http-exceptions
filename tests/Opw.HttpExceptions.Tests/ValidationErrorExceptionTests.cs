using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class ValidationErrorExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateInvalidModelException_With1Error()
        {
            var exception = new ValidationErrorException<string>("memberName", "error1", "error2");

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

            var exception = new ValidationErrorException<string>(errors);

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

            var exception = SerializationHelper.SerializeDeserialize(new ValidationErrorException<string>(errors));

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
            exception.Errors.Should().HaveCount(2);
        }
    }
}
