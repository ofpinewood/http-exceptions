using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Opw.HttpExceptions
{
    public class SerializableExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateSerializableException()
        {
            var exception = new SerializableException(new ApplicationException("ApplicationException", new ArgumentNullException("param", "ArgumentNullException")));

            exception.Type.Should().Be("ApplicationException");
            exception.Message.Should().Be("ApplicationException");
            exception.InnerException.Type.Should().StartWith("ArgumentNullException");
            exception.InnerException["ParamName"].Should().Be("param");
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = new SerializableException(new ApplicationException("ApplicationException", new ArgumentNullException("param", "ArgumentNullException")));
            exception = SerializationHelper.SerializeDeserialize(exception);

            exception.Type.Should().Be("ApplicationException");
            exception.Message.Should().Be("ApplicationException");
            exception.InnerException.Type.Should().StartWith("ArgumentNullException");
            exception.InnerException["ParamName"].Should().Be("param");
        }
    }
}
