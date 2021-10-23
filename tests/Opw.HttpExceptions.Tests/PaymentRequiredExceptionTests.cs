using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class PaymentRequiredExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreatePaymentRequiredException()
        {
            var exception = new PaymentRequiredException();

            exception.StatusCode.Should().Be(HttpStatusCode.PaymentRequired);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.PaymentRequired);
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = SerializationHelper.SerializeDeserialize(new PaymentRequiredException());

            exception.StatusCode.Should().Be(HttpStatusCode.PaymentRequired);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.PaymentRequired);
        }
    }
}
